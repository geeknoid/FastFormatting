// © Microsoft Corporation. All rights reserved.

namespace FastFormatting
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides highly efficient string formatting functionality.
    /// </summary>
    /// <remarks>
    /// This class lets you optimize string formatting operations common with the <see cref="String.Format"  />
    /// method. This is useful for any situation where you need to repeatedly format the same string with 
    /// different arguments.
    /// 
    /// This class works faster than String.Format because it parses the composite format string only once when
    /// the instance is created, rather than doing it for every formatting operation.
    /// 
    /// You first create an instance of this class, passing the composite format string that you intend to use.
    /// Once the instance is created, you can call the <see cref="Format"/> method with arguments to use in the
    /// format operation.
    /// 
    /// Note that if you're only formatting a single string, it is more efficient to just use String.Format. This 
    /// class is meant for repeated use.
    /// </remarks>
    public partial class StringFormatter
    {
        readonly Segment[] _segments;  // info on the different chunks to process
        readonly string _literalString;         // all literal text to be inserted into the output
        readonly int _numArgs;                  // # args needed during format

        private const int MaxStackAlloc = 128;  // = 256 bytes

        /// <summary>
        /// Parses a composite format string into an efficient form for later use.
        /// </summary>
        public StringFormatter(ReadOnlySpan<char> format)
        {
            if (format == null)
            {
                throw new ArgumentNullException(nameof(format));
            }

            int pos = 0;
            int len = format.Length;
            char ch = '\0';
            var segments = new List<Segment>();
            int numArgs = 0;
            var literal = new StringMaker(format.Length);

            for (; ; )
            {
                int segStart = literal.Length;
                while (pos < len)
                {
                    ch = format[pos];

                    pos++;
                    if (ch == '}')
                    {
                        if (pos < len && format[pos] == '}')
                        {
                            // double }, treat as escape sequence
                            pos++;
                        }
                        else
                        {
                            // dangling }
                            throw new FormatException("Dangling } in format string.");
                        }
                    }
                    else if (ch == '{')
                    {
                        if (pos < len && format[pos] == '{')
                        {
                            // double {, treat as escape sequence
                            pos++;
                        }
                        else
                        {
                            // start of a format specification
                            pos--;
                            break;
                        }
                    }

                    literal.Append(ch);
                }

                if (pos == len)
                {
                    int totalLit = literal.Length - segStart;
                    while (totalLit > 0)
                    {
                        int num = totalLit;
                        if (num > short.MaxValue)
                        {
                            num = short.MaxValue;
                        }

                        segments.Add(new Segment((short)num, -1, 0, string.Empty));
                        totalLit -= num;
                    }

                    // done
                    _literalString = literal.ExtractString();
                    _numArgs = numArgs;
                    _segments = segments.ToArray();
                    return;
                }

                pos++;
                if (pos == len || (ch = format[pos]) < '0' || ch > '9')
                {
                    // we need an argument index
                    throw new FormatException("Missing argument index in format string.");
                }

                // extract the argument index
                int argIndex = 0;
                do
                {
                    argIndex = (argIndex * 10) + (ch - '0');
                    pos++;

                    // make sure we get a suitable end to the argument index
                    if (pos == len)
                    {
                        throw new FormatException("Invalid argument index.");
                    }

                    ch = format[pos];
                } while (ch >= '0' && ch <= '9');

                if (argIndex >= numArgs)
                {
                    // new max arg count
                    numArgs = argIndex + 1;
                }

                // skip whitespace
                while (pos < len && (ch = format[pos]) == ' ')
                {
                    pos++;
                }

                // parse the optional field width
                bool leftAligned = false;
                int argWidth = 0;
                if (ch == ',')
                {
                    pos++;

                    // skip whitespace
                    while (pos < len && format[pos] == ' ')
                    {
                        pos++;
                    }

                    // did we run out of steam
                    if (pos == len)
                    {
                        throw new FormatException($"Invalid field width for argument {numArgs + 1}");
                    }

                    ch = format[pos];
                    if (ch == '-')
                    {
                        leftAligned = true;
                        pos++;

                        // did we run out of steam?
                        if (pos == len)
                        {
                            throw new FormatException($"Invalid field width for argument {numArgs + 1}");
                        }

                        ch = format[pos];
                    }

                    if (ch < '0' || ch > '9')
                    {
                        throw new FormatException($"Invalid character in field width for argument {numArgs + 1}.");
                    }

                    int val = 0;
                    do
                    {
                        val = (val * 10) + (ch - '0');
                        pos++;

                        // did we run out of steam?
                        if (pos == len)
                        {
                            throw new FormatException($"Invalid format string");
                        }

                        // did we get a number that's too big?
                        if (val > short.MaxValue)
                        {
                            throw new FormatException($"Field width value exceeds limit for argument {numArgs+1}.");
                        }

                        ch = format[pos];
                    } while (ch >= '0' && ch <= '9');

                    argWidth = val;
                }

                if (leftAligned)
                {
                    argWidth = -argWidth;
                }

                // skip whitespace
                while (pos < len && (ch = format[pos]) == ' ')
                {
                    pos++;
                }

                // parse the optional argument format string

                string argFormat = string.Empty;
                if (ch == ':')                {
                    pos++;
                    int argFormatStart = pos;

                    for (; ; )
                    {
                        if (pos == len)
                        {
                            throw new FormatException("Invalid format specification.");
                        }

                        ch = format[pos];
                        pos++;
                        if (ch == '{')
                        {
                            throw new FormatException("Nested { in format specification.");
                        }
                        else if (ch == '}')
                        {
                            // end of format specification
                            pos--;
                            break;
                        }
                    }

                    if (pos != argFormatStart)
                    {
                        argFormat = format.Slice(argFormatStart, pos - argFormatStart).ToString();
                    }
                }

                if (ch != '}')
                {
                    throw new FormatException("Unterminated format specification.");
                }

                // skip over the closing }
                pos++;

                if (numArgs >= short.MaxValue)
                {
                    throw new FormatException("Must have less than 32768 arguments");
                }

                int total = literal.Length - segStart;
                while (total > short.MaxValue)
                {
                    segments.Add(new Segment(short.MaxValue, -1, 0, string.Empty));
                    total -= short.MaxValue;
                }

                segments.Add(new Segment((short)total, (short)argIndex, (short)argWidth, argFormat));
            }
        }

        struct Nothing
        {
        }

        private void CheckNumArgs(int explicitCount, object?[]? args)
        {
            int total = explicitCount;
            if (args != null)
            {
                total += args.Length;
            }

            if (_numArgs != total)
            {
                throw new ArgumentException($"Expected {_numArgs} arguments, but got {total}");
            }
        }

        /// <summary>
        /// Format a string with a single argument.
        /// </summary>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="arg">An argument to use in the formatting operation.</param>
        /// <returns>The formatting string.</returns>
        public string Format<T>(IFormatProvider? provider, T arg)
        {
            CheckNumArgs(1, null);
            var pa = new Params<T, Nothing, Nothing>(arg, default(Nothing), default(Nothing));
            return Format(provider, in pa);
        }

        /// <summary>
        /// Formats a string with two arguments.
        /// </summary>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="arg0">An argument to use in the formatting operation.</param>
        /// <param name="arg1">An argument to use in the formatting operation.</param>
        /// <returns>The formatting string.</returns>
        public string Format<T0, T1>(IFormatProvider? provider, T0 arg0, T1 arg1)
        {
            CheckNumArgs(2, null);
            var pa = new Params<T0, T1, Nothing>(arg0, arg1, default(Nothing));
            return Format(provider, in pa);
        }

        /// <summary>
        /// Formats a string with three arguments.
        /// </summary>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="arg0">An argument to use in the formatting operation.</param>
        /// <param name="arg1">An argument to use in the formatting operation.</param>
        /// <param name="arg2">An argument to use in the formatting operation.</param>
        /// <returns>The formatting string.</returns>
        public string Format<T0, T1, T2>(IFormatProvider? provider, T0 arg0, T1 arg1, T2 arg2)
        {
            CheckNumArgs(3, null);
            var pa = new Params<T0, T1, T2>(arg0, arg1, arg2);
            return Format(provider, in pa);
        }

        /// <summary>
        /// Formats a string with arguments.
        /// </summary>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="arg0">An argument to use in the formatting operation.</param>
        /// <param name="arg1">An argument to use in the formatting operation.</param>
        /// <param name="arg2">An argument to use in the formatting operation.</param>
        /// <param name="args">Additional arguments to use in the formatting operation.</param>
        /// <returns>The formatting string.</returns>
        public string Format<T0, T1, T2>(IFormatProvider? provider, T0 arg0, T1 arg1, T2 arg2, params object?[]? args)
        {
            CheckNumArgs(3, args);
            var pa = new Params<T0, T1, T2>(arg0, arg1, arg2, args);
            return Format(provider, in pa);
        }

        /// <summary>
        /// Formats a string with arguments.
        /// </summary>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="args">Arguments to use in the formatting operation.</param>
        /// <returns>The formatting string.</returns>
        public string Format(IFormatProvider? provider, params object?[]? args)
        {
            CheckNumArgs(0, args);

            if (_numArgs == 0)
            {
                return _literalString;
            }

            Params<object?, object?, object?> pa;
            switch (args!.Length)
            {
                case 1:
                    pa = new Params<object?, object?, object?>(args[0], null, null);
                    break;

                case 2:
                    pa = new Params<object?, object?, object?>(args[0], args[1], null);
                    break;
                
                case 3:
                    pa = new Params<object?, object?, object?>(args[0], args[1], args[2]);
                    break;
                
                default:
                    pa = new Params<object?, object?, object?>(args[0], args[1], args[2], args.AsSpan(3));
                    break;
            }

            return Format(provider, in pa);
        }

        /// <summary>
        /// Formats a string with one argument.
        /// </summary>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="arg">An argument to use in the formatting operation.</param>
        /// <param name="destination">Where to write the resulting string.</param>
        /// <param name="charsWritten">The number of characters actually written to the destination span.</param>
        /// <returns>True if there was enough room in teh destination span for the resulting string..</returns>
        public bool TryFormat<T>(Span<char> destination, out int charsWritten, IFormatProvider? provider, T arg)
        {
            CheckNumArgs(1, null);
            var pa = new Params<T, Nothing, Nothing>(arg, default(Nothing), default(Nothing));
            return TryFormat(destination, out charsWritten, provider, in pa);
        }

        /// <summary>
        /// Formats a string with two arguments.
        /// </summary>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="arg0">An argument to use in the formatting operation.</param>
        /// <param name="arg1">An argument to use in the formatting operation.</param>
        /// <param name="destination">Where to write the resulting string.</param>
        /// <param name="charsWritten">The number of characters actually written to the destination span.</param>
        /// <returns>True if there was enough room in teh destination span for the resulting string..</returns>
        public bool TryFormat<T0, T1>(Span<char> destination, out int charsWritten, IFormatProvider? provider, T0 arg0, T1 arg1)
        {
            CheckNumArgs(2, null);
            var pa = new Params<T0, T1, Nothing>(arg0, arg1, default(Nothing));
            return TryFormat(destination, out charsWritten, provider, in pa);
        }

        /// <summary>
        /// Formats a string with three arguments.
        /// </summary>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="arg0">An argument to use in the formatting operation.</param>
        /// <param name="arg1">An argument to use in the formatting operation.</param>
        /// <param name="arg2">An argument to use in the formatting operation.</param>
        /// <param name="destination">Where to write the resulting string.</param>
        /// <param name="charsWritten">The number of characters actually written to the destination span.</param>
        /// <returns>True if there was enough room in teh destination span for the resulting string..</returns>
        public bool TryFormat<T0, T1, T2>(Span<char> destination, out int charsWritten, IFormatProvider? provider, T0 arg0, T1 arg1, T2 arg2)
        {
            CheckNumArgs(3, null);
            var pa = new Params<T0, T1, T2>(arg0, arg1, arg2);
            return TryFormat(destination, out charsWritten, provider, in pa);
        }

        /// <summary>
        /// Formats a string with arguments.
        /// </summary>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="arg0">An argument to use in the formatting operation.</param>
        /// <param name="arg1">An argument to use in the formatting operation.</param>
        /// <param name="arg2">An argument to use in the formatting operation.</param>
        /// <param name="args">Additional arguments to use in the formatting operation.</param>
        /// <param name="destination">Where to write the resulting string.</param>
        /// <param name="charsWritten">The number of characters actually written to the destination span.</param>
        /// <returns>True if there was enough room in teh destination span for the resulting string..</returns>
        public bool TryFormat<T0, T1, T2>(Span<char> destination, out int charsWritten, IFormatProvider? provider, T0 arg0, T1 arg1, T2 arg2, params object?[]? args)
        {
            CheckNumArgs(3, args);
            var pa = new Params<T0, T1, T2>(arg0, arg1, arg2, args);
            return TryFormat(destination, out charsWritten, provider, in pa);
        }

        /// <summary>
        /// Formats a string with arguments.
        /// </summary>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="args">Arguments to use in the formatting operation.</param>
        /// <param name="destination">Where to write the resulting string.</param>
        /// <param name="charsWritten">The number of characters actually written to the destination span.</param>
        /// <returns>True if there was enough room in teh destination span for the resulting string..</returns>
        public bool TryFormat(Span<char> destination, out int charsWritten, IFormatProvider? provider, params object?[]? args)
        {
            CheckNumArgs(0, args);

            if (_numArgs == 0)
            {
                if (destination.Length < _literalString.Length)
                {
                    charsWritten = 0;
                    return false;
                }
                _literalString.AsSpan().CopyTo(destination);
                charsWritten = _literalString.Length;;
                return true;
            }

            Params<object?, object?, object?> pa;
            switch (args!.Length)
            {
                case 1:
                    pa = new Params<object?, object?, object?>(args[0], null, null);
                    break;

                case 2:
                    pa = new Params<object?, object?, object?>(args[0], args[1], null);
                    break;
                
                case 3:
                    pa = new Params<object?, object?, object?>(args[0], args[1], args[2]);
                    break;
                
                default:
                    pa = new Params<object?, object?, object?>(args[0], args[1], args[2], args.AsSpan(3));
                    break;
            }

            return TryFormat(destination, out charsWritten, provider, in pa);
        }

        private string Format<T0, T1, T2>(IFormatProvider? provider, in Params<T0, T1, T2> pa)
        {
            int estimatedSize = EstimateResultSize(in pa);
            var sm = (estimatedSize >= MaxStackAlloc) ? new StringMaker(estimatedSize) : new StringMaker(stackalloc char[MaxStackAlloc]);
            Format<T0, T1, T2>(ref sm, provider, in pa);
            return sm.ExtractString();
        }

        private bool TryFormat<T0, T1, T2>(Span<char> destination, out int charsWritten, IFormatProvider? provider, in Params<T0, T1, T2> pa)
        {
            var sm = new StringMaker(destination, false);
            Format<T0, T1, T2>(ref sm, provider, in pa);
            charsWritten = sm.Length;
            var overflowed = sm.Overflowed;
            sm.Dispose();
            return !overflowed;
        }

        // Given this has 3 generics, it can lead to a lot of jitted code. We thus keep
        // the work done in here to a strict minimum, and dispatch to lower-arity methods
        // ASAP.
        //
        // This code assumes there are sufficient arguments in the ParamsArray to satisfy the needs
        // of the format operation, so the upstream callers should validate this a priori.
        private void Format<T0, T1, T2>(ref StringMaker sm, IFormatProvider? provider, in Params<T0, T1, T2> pa)
        {
            int literalIndex = 0;
            foreach (var segment in _segments)
            {
                int literalCount = segment.LiteralCount;
                if (literalCount > 0)
                {
                    // the segment has some literal text
                    sm.Append(_literalString.AsSpan(literalIndex, literalCount));
                    literalIndex += literalCount;
                }

                var argIndex = segment.ArgIndex;
                if (argIndex >= 0)
                {
                    // the segment has an arg to format
                    switch (argIndex)
                    {
                        case 0:
                            AppendArg(ref sm, pa.Arg0, segment.ArgFormat, segment.ArgWidth, provider);
                            break;

                        case 1:
                            AppendArg(ref sm, pa.Arg1, segment.ArgFormat, segment.ArgWidth, provider);
                            break;

                        case 2:
                            AppendArg(ref sm, pa.Arg2, segment.ArgFormat, segment.ArgWidth, provider);
                            break;

                        default:
                            AppendReferenceArg(ref sm, pa.Args[argIndex - 3], segment.ArgFormat, segment.ArgWidth, provider);
                            break;
                    }
                }
            }
        }

        private void AppendArg<T>(ref StringMaker sm, T arg, string argFormat, int argWidth, IFormatProvider? provider)
        {
            switch (arg)
            {
                case int a:
                    sm.Append(a, argFormat, provider, argWidth);
                    break;

                case long a:
                    sm.Append(a, argFormat, provider, argWidth);
                    break;

                case double a:
                    sm.Append(a, argFormat, provider, argWidth);
                    break;

                case float a:
                    sm.Append(a, argFormat, provider, argWidth);
                    break;

                case uint a:
                    sm.Append(a, argFormat, provider, argWidth);
                    break;

                case ulong a:
                    sm.Append(a, argFormat, provider, argWidth);
                    break;

                case short a:
                    sm.Append(a, argFormat, provider, argWidth);
                    break;

                case ushort a:
                    sm.Append(a, argFormat, provider, argWidth);
                    break;

                case byte a:
                    sm.Append(a, argFormat, provider, argWidth);
                    break;

                case sbyte a:
                    sm.Append(a, argFormat, provider, argWidth);
                    break;

                case bool a:
                    sm.Append(a, argWidth);
                    break;

                case char a:
                    sm.Append(a, argWidth);
                    break;

                case decimal a:
                    sm.Append(a, argFormat, provider, argWidth);
                    break;

                case DateTime a:
                    sm.Append(a, argFormat, provider, argWidth);
                    break;

                case TimeSpan a:
                    sm.Append(a, argFormat, provider, argWidth);
                    break;

                default:
                    AppendReferenceArg(ref sm, arg, argFormat, argWidth, provider);
                    break;
            }
        }

        private void AppendReferenceArg(ref StringMaker sm, object? arg, string argFormat, int argWidth, IFormatProvider? provider)
        {
            switch (arg)
            {
                case string a:
                    sm.Append(a, argWidth);
                    break;

                case ISpanFormattable a:
                    sm.Append(a, argFormat, provider, argWidth);
                    break;

                case IFormattable a:
                    sm.Append(a, argFormat, provider, argWidth);
                    break;

                case object a:
                    sm.Append(a, argWidth);
                    break;

                default:
                    // when arg == null
                    sm.Append(string.Empty, argWidth);
                    break;
            }
        }

        private int EstimateResultSize<T0, T1, T2>(in Params<T0, T1, T2> pa)
        {
            // make a guesstimate at the size of the buffer we need for output
            int estimatedSize = _literalString.Length + _numArgs * 16;

            if (typeof(T0) == typeof(string))
            {
                var str = pa.Arg0 as string;
                if (str != null)
                {
                    estimatedSize += str.Length;
                }
            }

            if (typeof(T1) == typeof(string))
            {
                var str = pa.Arg1 as string;
                if (str != null)
                {
                    estimatedSize += str.Length;
                }
            }

            if (typeof(T2) == typeof(string))
            {
                var str = pa.Arg2 as string;
                if (str != null)
                {
                    estimatedSize += str.Length;
                }
            }

            foreach (var arg in pa.Args)
            {
                if (arg is string s)
                {
                    estimatedSize += s.Length;
                }
            }

            return estimatedSize;
        }

        /// <summary>
        /// Gets the number of arguments required in order to produce a string with this instance.
        /// </summary>
        public int NumArgumentsNeeded => _numArgs;
    }
}
