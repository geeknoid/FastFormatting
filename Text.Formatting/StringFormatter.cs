// © Microsoft Corporation. All rights reserved.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Text.Formatting.Test")]
[assembly: InternalsVisibleTo("Text.Formatting.Bench")]

namespace Text
{
#pragma warning disable SA1011 // Closing square brackets should be spaced correctly
#pragma warning disable S109 // Magic numbers should not be used
#pragma warning disable S2302 // "nameof" should be used

    /// <summary>
    /// Provides highly efficient string formatting functionality.
    /// </summary>
    /// <remarks>
    /// This type lets you optimize string formatting operations common with the <see cref="string.Format(string,object?)"  />
    /// method. This is useful for any situation where you need to repeatedly format the same string with
    /// different arguments.
    ///
    /// This type works faster than <c>string.Format</c> because it parses the composite format string only once when
    /// the instance is created, rather than doing it for every formatting operation.
    ///
    /// You first create an instance of this type, passing the composite format string that you intend to use.
    /// Once the instance is created, you call the <see cref="Format{T}(IFormatProvider?,T)"/> method with arguments to use in the
    /// format operation.
    /// </remarks>
    public readonly partial struct StringFormatter
    {
        private const int MaxStackAlloc = 128;  // = 256 bytes

        private readonly Segment[] _segments;     // info on the different chunks to process
        private readonly string _literalString;   // all literal text to be inserted into the output

        /// <summary>
        /// Initializes a new instance of the <see cref="StringFormatter"/> struct.
        /// </summary>
        /// <param name="format">A classic .NET format string as used with <see cref="string.Format(string,object?)"  />.</param>
        /// <remarks>
        /// Parses a composite format string into an efficient form for later use.
        /// </remarks>
        public StringFormatter(ReadOnlySpan<char> format)
        {
            var pos = 0;
            var len = format.Length;
            var ch = '\0';
            var segments = new List<Segment>();
            var numArgs = 0;
            using var literal = (format.Length >= MaxStackAlloc) ? new StringMaker(format.Length) : new StringMaker(stackalloc char[MaxStackAlloc]);

            while (true)
            {
                var segStart = literal.Length;
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
                            // dangling }, fail
                            throw new ArgumentException($"Dangling }} in format string at position {pos}", nameof(format));
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
                    var totalLit = literal.Length - segStart;
                    while (totalLit > 0)
                    {
                        var num = totalLit;
                        if (num > short.MaxValue)
                        {
                            num = short.MaxValue;
                        }

                        segments.Add(new Segment((short)num, -1, 0, string.Empty));
                        totalLit -= num;
                    }

                    // done
                    _literalString = literal.ExtractString();
                    NumArgumentsNeeded = numArgs;
                    _segments = segments.ToArray();
                    return;
                }

                pos++;
                if (pos == len || (ch = format[pos]) < '0' || ch > '9')
                {
                    // we need an argument index
                    throw new ArgumentException($"Missing argument index in format string at position {pos}", nameof(format));
                }

                // extract the argument index
                var argIndex = 0;
                do
                {
                    argIndex = (argIndex * 10) + (ch - '0');
                    pos++;

                    // make sure we get a suitable end to the argument index
                    if (pos == len)
                    {
                        throw new ArgumentException($"Invalid argument index in format string at position {pos}", nameof(format));
                    }

                    ch = format[pos];
                }
                while (ch >= '0' && ch <= '9');

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
                var leftAligned = false;
                var argWidth = 0;
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
                        throw new ArgumentException($"Invalid field width for argument {numArgs + 1} in format string", nameof(format));
                    }

                    ch = format[pos];
                    if (ch == '-')
                    {
                        leftAligned = true;
                        pos++;

                        // did we run out of steam?
                        if (pos == len)
                        {
                            throw new ArgumentException($"Invalid field width for argument {numArgs + 1} in format string", nameof(format));
                        }

                        ch = format[pos];
                    }

                    if (ch < '0' || ch > '9')
                    {
                        throw new ArgumentException($"Invalid character in field width for argument {numArgs + 1} in format string", nameof(format));
                    }

                    var val = 0;
                    do
                    {
                        val = (val * 10) + (ch - '0');
                        pos++;

                        // did we run out of steam?
                        if (pos == len)
                        {
                            throw new ArgumentException($"Incomplete field width at position {pos}", nameof(format));
                        }

                        // did we get a number that's too big?
                        if (val > short.MaxValue)
                        {
                            throw new ArgumentException($"Field width value exceeds limit for argument {numArgs + 1} in format string", nameof(format));
                        }

                        ch = format[pos];
                    }
                    while (ch >= '0' && ch <= '9');

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

                var argFormat = string.Empty;
                if (ch == ':')
                {
                    pos++;
                    var argFormatStart = pos;

                    while (true)
                    {
                        if (pos == len)
                        {
                            throw new ArgumentException($"Unterminated format specification at position {pos}", nameof(format));
                        }

                        ch = format[pos];
                        pos++;
                        if (ch == '{')
                        {
                            throw new ArgumentException($"Nested {{ in format specification at position {pos}", nameof(format));
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
                    throw new ArgumentException("Unterminated format specification", nameof(format));
                }

                // skip over the closing brace
                pos++;

                if (numArgs >= short.MaxValue)
                {
                    throw new ArgumentException("Must have less than 32768 arguments", nameof(format));
                }

                var total = literal.Length - segStart;
                while (total > short.MaxValue)
                {
                    segments.Add(new Segment(short.MaxValue, -1, 0, string.Empty));
                    total -= short.MaxValue;
                }

                segments.Add(new Segment((short)total, (short)argIndex, (short)argWidth, argFormat));
            }
        }

        /// <summary>
        /// Formats a string with a single argument.
        /// </summary>
        /// <typeparam name="T">Type of the single argument.</typeparam>
        /// <param name="arg">An argument to use in the formatting operation.</param>
        /// <returns>The formatted string.</returns>
        public string Format<T>(T arg) => Format<T>(null, arg);

        /// <summary>
        /// Formats a string with a single argument.
        /// </summary>
        /// <typeparam name="T">Type of the single argument.</typeparam>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="arg">An argument to use in the formatting operation.</param>
        /// <returns>The formatted string.</returns>
        public string Format<T>(IFormatProvider? provider, T arg)
        {
            CheckNumArgs(1, null);
            var pa = new Params<T, Nothing, Nothing>(arg, default, default);
            return Format(provider, in pa);
        }

        /// <summary>
        /// Formats a string with two arguments.
        /// </summary>
        /// <typeparam name="T0">Type of the first argument.</typeparam>
        /// <typeparam name="T1">Type of the second argument.</typeparam>
        /// <param name="arg0">First argument to use in the formatting operation.</param>
        /// <param name="arg1">Second argument to use in the formatting operation.</param>
        /// <returns>The formatted string.</returns>
        public string Format<T0, T1>(T0 arg0, T1 arg1) => Format<T0, T1>(null, arg0, arg1);

        /// <summary>
        /// Formats a string with two arguments.
        /// </summary>
        /// <typeparam name="T0">Type of the first argument.</typeparam>
        /// <typeparam name="T1">Type of the second argument.</typeparam>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="arg0">First argument to use in the formatting operation.</param>
        /// <param name="arg1">Second argument to use in the formatting operation.</param>
        /// <returns>The formatted string.</returns>
        public string Format<T0, T1>(IFormatProvider? provider, T0 arg0, T1 arg1)
        {
            CheckNumArgs(2, null);
            var pa = new Params<T0, T1, Nothing>(arg0, arg1, default);
            return Format(provider, in pa);
        }

        /// <summary>
        /// Formats a string with three arguments.
        /// </summary>
        /// <typeparam name="T0">Type of the first argument.</typeparam>
        /// <typeparam name="T1">Type of the second argument.</typeparam>
        /// <typeparam name="T2">Type of the third argument.</typeparam>
        /// <param name="arg0">First argument to use in the formatting operation.</param>
        /// <param name="arg1">Second argument to use in the formatting operation.</param>
        /// <param name="arg2">Third argument to use in the formatting operation.</param>
        /// <returns>The formatted string.</returns>
        public string Format<T0, T1, T2>(T0 arg0, T1 arg1, T2 arg2) => Format<T0, T1, T2>(null, arg0, arg1, arg2);

        /// <summary>
        /// Formats a string with three arguments.
        /// </summary>
        /// <typeparam name="T0">Type of the first argument.</typeparam>
        /// <typeparam name="T1">Type of the second argument.</typeparam>
        /// <typeparam name="T2">Type of the third argument.</typeparam>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="arg0">First argument to use in the formatting operation.</param>
        /// <param name="arg1">Second argument to use in the formatting operation.</param>
        /// <param name="arg2">Third argument to use in the formatting operation.</param>
        /// <returns>The formatted string.</returns>
        public string Format<T0, T1, T2>(IFormatProvider? provider, T0 arg0, T1 arg1, T2 arg2)
        {
            CheckNumArgs(3, null);
            var pa = new Params<T0, T1, T2>(arg0, arg1, arg2);
            return Format(provider, in pa);
        }

        /// <summary>
        /// Formats a string with arguments.
        /// </summary>
        /// <typeparam name="T0">Type of the first argument.</typeparam>
        /// <typeparam name="T1">Type of the second argument.</typeparam>
        /// <typeparam name="T2">Type of the third argument.</typeparam>
        /// <param name="arg0">First argument to use in the formatting operation.</param>
        /// <param name="arg1">Second argument to use in the formatting operation.</param>
        /// <param name="arg2">Third argument to use in the formatting operation.</param>
        /// <param name="args">Additional arguments to use in the formatting operation.</param>
        /// <returns>The formatted string.</returns>
        public string Format<T0, T1, T2>(T0 arg0, T1 arg1, T2 arg2, params object?[]? args) => Format<T0, T1, T2>(null, arg0, arg1, arg2, args);

        /// <summary>
        /// Formats a string with arguments.
        /// </summary>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <typeparam name="T0">Type of the first argument.</typeparam>
        /// <typeparam name="T1">Type of the second argument.</typeparam>
        /// <typeparam name="T2">Type of the third argument.</typeparam>
        /// <param name="arg0">First argument to use in the formatting operation.</param>
        /// <param name="arg1">Second argument to use in the formatting operation.</param>
        /// <param name="arg2">Third argument to use in the formatting operation.</param>
        /// <param name="args">Additional arguments to use in the formatting operation.</param>
        /// <returns>The formatted string.</returns>
        public string Format<T0, T1, T2>(IFormatProvider? provider, T0 arg0, T1 arg1, T2 arg2, params object?[]? args)
        {
            CheckNumArgs(3, args);
            var pa = new Params<T0, T1, T2>(arg0, arg1, arg2, args);
            return Format(provider, in pa);
        }

        /// <summary>
        /// Formats a string with arguments.
        /// </summary>
        /// <param name="args">Arguments to use in the formatting operation.</param>
        /// <returns>The formatted string.</returns>
        public string Format(params object?[]? args) => Format(null, args);

        /// <summary>
        /// Formats a string with arguments.
        /// </summary>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="args">Arguments to use in the formatting operation.</param>
        /// <returns>The formatted string.</returns>
        public string Format(IFormatProvider? provider, params object?[]? args)
        {
            CheckNumArgs(0, args);

            if (NumArgumentsNeeded == 0)
            {
                return _literalString;
            }

            var pa = args!.Length switch
            {
                1 => new Params<object?, object?, object?>(args[0], null, null),
                2 => new Params<object?, object?, object?>(args[0], args[1], null),
                3 => new Params<object?, object?, object?>(args[0], args[1], args[2]),
                _ => new Params<object?, object?, object?>(args[0], args[1], args[2], args.AsSpan(3))
            };

            return Format(provider, in pa);
        }

        /// <summary>
        /// Formats a string with one argument.
        /// </summary>
        /// <typeparam name="T">Type of the single argument.</typeparam>
        /// <param name="destination">Where to write the resulting string.</param>
        /// <param name="charsWritten">The number of characters actually written to the destination span.</param>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="arg">An argument to use in the formatting operation.</param>
        /// <returns>True if there was enough room in teh destination span for the resulting string.</returns>
        public bool TryFormat<T>(Span<char> destination, out int charsWritten, IFormatProvider? provider, T arg)
        {
            CheckNumArgs(1, null);
            var pa = new Params<T, Nothing, Nothing>(arg, default, default);
            return TryFormat(destination, out charsWritten, provider, in pa);
        }

        /// <summary>
        /// Formats a string with two arguments.
        /// </summary>
        /// <typeparam name="T0">Type of the first argument.</typeparam>
        /// <typeparam name="T1">Type of the second argument.</typeparam>
        /// <param name="destination">Where to write the resulting string.</param>
        /// <param name="charsWritten">The number of characters actually written to the destination span.</param>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="arg0">First argument to use in the formatting operation.</param>
        /// <param name="arg1">Second argument to use in the formatting operation.</param>
        /// <returns>True if there was enough room in teh destination span for the resulting string.</returns>
        public bool TryFormat<T0, T1>(Span<char> destination, out int charsWritten, IFormatProvider? provider, T0 arg0, T1 arg1)
        {
            CheckNumArgs(2, null);
            var pa = new Params<T0, T1, Nothing>(arg0, arg1, default);
            return TryFormat(destination, out charsWritten, provider, in pa);
        }

        /// <summary>
        /// Formats a string with three arguments.
        /// </summary>
        /// <typeparam name="T0">Type of the first argument.</typeparam>
        /// <typeparam name="T1">Type of the second argument.</typeparam>
        /// <typeparam name="T2">Type of the third argument.</typeparam>
        /// <param name="destination">Where to write the resulting string.</param>
        /// <param name="charsWritten">The number of characters actually written to the destination span.</param>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="arg0">First argument to use in the formatting operation.</param>
        /// <param name="arg1">Second argument to use in the formatting operation.</param>
        /// <param name="arg2">Third argument to use in the formatting operation.</param>
        /// <returns>True if there was enough room in teh destination span for the resulting string.</returns>
        public bool TryFormat<T0, T1, T2>(Span<char> destination, out int charsWritten, IFormatProvider? provider, T0 arg0, T1 arg1, T2 arg2)
        {
            CheckNumArgs(3, null);
            var pa = new Params<T0, T1, T2>(arg0, arg1, arg2);
            return TryFormat(destination, out charsWritten, provider, in pa);
        }

        /// <summary>
        /// Formats a string with arguments.
        /// </summary>
        /// <typeparam name="T0">Type of the first argument.</typeparam>
        /// <typeparam name="T1">Type of the second argument.</typeparam>
        /// <typeparam name="T2">Type of the third argument.</typeparam>
        /// <param name="destination">Where to write the resulting string.</param>
        /// <param name="charsWritten">The number of characters actually written to the destination span.</param>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="arg0">First argument to use in the formatting operation.</param>
        /// <param name="arg1">Second argument to use in the formatting operation.</param>
        /// <param name="arg2">Third argument to use in the formatting operation.</param>
        /// <param name="args">Additional arguments to use in the formatting operation.</param>
        /// <returns>True if there was enough room in teh destination span for the resulting string.</returns>
        public bool TryFormat<T0, T1, T2>(Span<char> destination, out int charsWritten, IFormatProvider? provider, T0 arg0, T1 arg1, T2 arg2, params object?[]? args)
        {
            CheckNumArgs(3, args);
            var pa = new Params<T0, T1, T2>(arg0, arg1, arg2, args);
            return TryFormat(destination, out charsWritten, provider, in pa);
        }

        /// <summary>
        /// Formats a string with arguments.
        /// </summary>
        /// <param name="destination">Where to write the resulting string.</param>
        /// <param name="charsWritten">The number of characters actually written to the destination span.</param>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="args">Arguments to use in the formatting operation.</param>
        /// <returns>True if there was enough room in teh destination span for the resulting string.</returns>
        public bool TryFormat(Span<char> destination, out int charsWritten, IFormatProvider? provider, params object?[]? args)
        {
            CheckNumArgs(0, args);

            if (NumArgumentsNeeded == 0)
            {
                if (destination.Length < _literalString.Length)
                {
                    charsWritten = 0;
                    return false;
                }

                _literalString.AsSpan().CopyTo(destination);
                charsWritten = _literalString.Length;
                return true;
            }

            var pa = args!.Length switch
            {
                1 => new Params<object?, object?, object?>(args[0], null, null),
                2 => new Params<object?, object?, object?>(args[0], args[1], null),
                3 => new Params<object?, object?, object?>(args[0], args[1], args[2]),
                _ => new Params<object?, object?, object?>(args[0], args[1], args[2], args.AsSpan(3))
            };

            return TryFormat(destination, out charsWritten, provider, in pa);
        }

        private static void AppendArg<T>(ref StringMaker sm, T arg, string argFormat, int argWidth, IFormatProvider? provider)
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

        private static void AppendReferenceArg(ref StringMaker sm, object? arg, string argFormat, int argWidth, IFormatProvider? provider)
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

        private bool TryFormat<T0, T1, T2>(Span<char> destination, out int charsWritten, IFormatProvider? provider, in Params<T0, T1, T2> pa)
        {
            var sm = new StringMaker(destination, true);
            Format<T0, T1, T2>(ref sm, provider, in pa);
            charsWritten = sm.Length;
            var overflowed = sm.Overflowed;
            sm.Dispose();
            return !overflowed;
        }

        private string Format<T0, T1, T2>(IFormatProvider? provider, in Params<T0, T1, T2> pa)
        {
            var estimatedSize = EstimateResultSize(in pa);
            var sm = (estimatedSize >= MaxStackAlloc) ? new StringMaker(estimatedSize) : new StringMaker(stackalloc char[MaxStackAlloc]);
            Format<T0, T1, T2>(ref sm, provider, in pa);
            var result = sm.ExtractString();
            sm.Dispose();
            return result;
        }

        // Given this has 3 generics, it can lead to a lot of jitted code. We thus keep
        // the work done in here to a strict minimum, and dispatch to lower-arity methods
        // ASAP.
        //
        // This code assumes there are sufficient arguments in the ParamsArray to satisfy the needs
        // of the format operation, so the upstream callers should validate this a priori.
        private void Format<T0, T1, T2>(ref StringMaker sm, IFormatProvider? provider, in Params<T0, T1, T2> pa)
        {
            var literalIndex = 0;
            foreach (var segment in _segments)
            {
                int literalCount = segment.LiteralCount;
                if (literalCount > 0)
                {
                    // the segment has some literal text
                    sm.Append(_literalString.AsSpan(literalIndex, literalCount), 0);
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

        private int EstimateResultSize<T0, T1, T2>(in Params<T0, T1, T2> pa)
        {
            // make a guesstimate at the size of the buffer we need for output
            var estimatedSize = _literalString.Length + (NumArgumentsNeeded * 16);

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
                if (arg is string str)
                {
                    estimatedSize += str.Length;
                }
            }

            return estimatedSize;
        }

        /// <summary>
        /// Gets the number of arguments required in order to produce a string with this instance.
        /// </summary>
        public int NumArgumentsNeeded { get; }

        private struct Nothing
        {
        }

        private void CheckNumArgs(int explicitCount, object?[]? args)
        {
            var total = explicitCount;
            if (args != null)
            {
                total += args.Length;
            }

            if (NumArgumentsNeeded != total)
            {
                throw new ArgumentException($"Expected {NumArgumentsNeeded} arguments, but got {total}");
            }
        }
    }
}
