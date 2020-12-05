// © Microsoft Corporation. All rights reserved.

namespace System.Text
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
        readonly FormatterSegment[] _segments;
        readonly string _literalString;
        readonly int _numArgs;

        private const int MaxStackAlloc = 128;  // = 256 bytes

        /// <summary>
        /// Parses a composite format string into an efficient form for later use.
        /// </summary>
        public StringFormatter(string format)
        {
            if (format == null)
            {
                throw new ArgumentNullException(nameof(format));
            }

            int pos = 0;
            int len = format.Length;
            char ch = '\0';
            var segments = new List<FormatterSegment>();
            int numArgs = 0;
            var literal = new ValueStringBuilder(format.Length);

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

                        segments.Add(new FormatterSegment(string.Empty, 0, (short)num, -1));
                        totalLit -= num;
                    }

                    // done
                    _literalString = literal.ToString();
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
                int index = 0;
                do
                {
                    index = (index * 10) + (ch - '0');
                    pos++;

                    // make sure we get a suitable end to the argument index
                    if (pos == len)
                    {
                        throw new FormatException("Invalid argument index.");
                    }

                    ch = format[pos];
                } while (ch >= '0' && ch <= '9');

                if (index >= numArgs)
                {
                    // new max arg count
                    numArgs = index + 1;
                }

                // skip whitespace
                while (pos < len && (ch = format[pos]) == ' ')
                {
                    pos++;
                }

                // parse the optional field width
                bool leftJustify = false;
                int width = 0;
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
                        throw new FormatException(SR.Format_InvalidString);
                    }

                    ch = format[pos];
                    if (ch == '-')
                    {
                        leftJustify = true;
                        pos++;

                        // did we run out of steam?
                        if (pos == len)
                        {
                            throw new FormatException(SR.Format_InvalidString);
                        }

                        ch = format[pos];
                    }

                    if (ch < '0' || ch > '9')
                    {
                        throw new FormatException("Invalid character in field width specification.");
                    }

                    int val = 0;
                    do
                    {
                        val = (val * 10) + (ch - '0');
                        pos++;

                        // did we run out of steam?
                        if (pos == len)
                        {
                            throw new FormatException(SR.Format_InvalidString);
                        }

                        // did we get a number that's too big?
                        if (val > short.MaxValue)
                        {
                            throw new FormatException("Field width value exceeds limit.");
                        }

                        ch = format[pos];
                    } while (ch >= '0' && ch <= '9');

                    width = val;
                }

                // skip whitespace
                while (pos < len && (ch = format[pos]) == ' ')
                {
                    pos++;
                }

                // parse the optional custom format string

                string fmtStr = string.Empty;
                if (ch == ':')
                {
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
                            if (pos < len && format[pos] == '{')
                            {
                                // double {, an escape sequence
                                pos++;
                            }
                            else
                            {
                                throw new FormatException("Nested { in format specification.");
                            }
                        }
                        else if (ch == '}')
                        {
                            if (pos < len && format[pos] == '}')
                            {
                                // double }, an escape sequence
                                pos++;
                            }
                            else
                            {
                                // end of format specification
                                pos--;
                                break;
                            }
                        }
                    }

                    if (pos != argFormatStart)
                    {
                        fmtStr = format.Substring(argFormatStart, pos - argFormatStart);
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

                if (!leftJustify)
                {
                    width = -width;
                }

                int total = literal.Length - segStart;
                while (total > short.MaxValue)
                {
                    segments.Add(new FormatterSegment(string.Empty, 0, short.MaxValue, -1));
                    total -= short.MaxValue;
                }

                segments.Add(new FormatterSegment(fmtStr, (short)width, (short)total, (short)index));
            }
        }

        struct Nothing
        {
        }

        /// <summary>
        /// Format a string with a single argument.
        /// </summary>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="arg">An argument to use in the formatting operation.</param>
        /// <returns>The formatting string.</returns>
        public string Format<T>(IFormatProvider? provider, T arg)
        {
            if (_numArgs != 1)
            {
                throw new ArgumentException($"Expected {_numArgs} arguments, but got 1");
            }

            var pa = new ParamsArray<T, Nothing, Nothing>(arg, default(Nothing), default(Nothing), 1);
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
            if (_numArgs != 2)
            {
                throw new ArgumentException($"Expected {_numArgs} arguments, but got 2");
            }

            var pa = new ParamsArray<T0, T1, Nothing>(arg0, arg1, default(Nothing), 2);
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
            if (_numArgs != 3)
            {
                throw new ArgumentException($"Expected {_numArgs} arguments, but got 3");
            }

            var pa = new ParamsArray<T0, T1, T2>(arg0, arg1, arg2, 3);
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
            if (args == null)
            {
                args = Array.Empty<object>();
            }

            var suppliedArgs = 3 + args.Length;
            if (_numArgs != suppliedArgs)
            {
                throw new ArgumentException($"Expected {_numArgs} arguments, but got {suppliedArgs}", nameof(args));
            }

            var pa = new ParamsArray<T0, T1, T2>(arg0, arg1, arg2, args);
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
            if (args == null || args.Length == 0)
            {
                if (_numArgs != 0)
                {
                    throw new ArgumentException($"Expected {_numArgs} arguments, but got 0", nameof(args));
                }
                return _literalString;
            }

            if (_numArgs != args.Length)
            {
                throw new ArgumentException($"Expected {_numArgs} arguments, but got {args.Length}", nameof(args));
            }

            ParamsArray<object?, object?, object?> pa;
            switch (args.Length)
            {
                case 1:
                    pa = new ParamsArray<object?, object?, object?>(args[0], null, null, 1);
                    break;
                case 2:
                    pa = new ParamsArray<object?, object?, object?>(args[0], args[1], null, 2);
                    break;
                case 3:
                    pa = new ParamsArray<object?, object?, object?>(args[0], args[1], args[2], 3);
                    break;
                default:
                    pa = new ParamsArray<object?, object?, object?>(args[0], args[1], args[2], args.AsSpan(3));
                    break;
            }

            return Format(provider, in pa);
        }

        private string Format<T0, T1, T2>(IFormatProvider? provider, in ParamsArray<T0, T1, T2> pa)
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

            var formatter = (estimatedSize >= MaxStackAlloc) ? new ValueStringBuilder(estimatedSize) : new ValueStringBuilder(stackalloc char[MaxStackAlloc]);
            formatter.Format<T0, T1, T2>(provider, in pa, _segments, _literalString);
            return formatter.ToString();
        }

        public bool TryFormat<T>(Span<char> destination, out int charsWritten, IFormatProvider? provider, T arg)
        {
            if (_numArgs != 1)
            {
                throw new ArgumentException($"Expected {_numArgs} arguments, but got 1");
            }

            var pa = new ParamsArray<T, Nothing, Nothing>(arg, default(Nothing), default(Nothing), 1);
            return TryFormat(destination, out charsWritten, provider, in pa);
        }

        public bool TryFormat<T0, T1>(Span<char> destination, out int charsWritten, IFormatProvider? provider, T0 arg0, T1 arg1)
        {
            if (_numArgs != 2)
            {
                throw new ArgumentException($"Expected {_numArgs} arguments, but got 2");
            }

            var pa = new ParamsArray<T0, T1, Nothing>(arg0, arg1, default(Nothing), 2);
            return TryFormat(destination, out charsWritten, provider, in pa);
        }

        public bool TryFormat<T0, T1, T2>(Span<char> destination, out int charsWritten, IFormatProvider? provider, T0 arg0, T1 arg1, T2 arg2)
        {
            if (_numArgs != 3)
            {
                throw new ArgumentException($"Expected {_numArgs} arguments, but got 3");
            }

            var pa = new ParamsArray<T0, T1, T2>(arg0, arg1, arg2, 3);
            return TryFormat(destination, out charsWritten, provider, in pa);
        }

        public bool TryFormat<T0, T1, T2>(Span<char> destination, out int charsWritten, IFormatProvider? provider, T0 arg0, T1 arg1, T2 arg2, params object?[]? args)
        {
            if (args == null)
            {
                args = Array.Empty<object>();
            }

            var suppliedArgs = 3 + args.Length;
            if (_numArgs != suppliedArgs)
            {
                throw new ArgumentException($"Expected {_numArgs} arguments, but got {suppliedArgs}", nameof(args));
            }

            var pa = new ParamsArray<T0, T1, T2>(arg0, arg1, arg2, args);
            return TryFormat(destination, out charsWritten, provider, in pa);
        }

        public bool TryFormat(Span<char> destination, out int charsWritten, IFormatProvider? provider, params object?[]? args)
        {
            if (args == null || args.Length == 0)
            {
                if (_numArgs != 0)
                {
                    throw new ArgumentException($"Expected {_numArgs} arguments, but got 0", nameof(args));
                }

                _literalString.AsSpan().CopyTo(destination);
                charsWritten = Math.Min(_literalString.Length, destination.Length);
                return true;
            }

            if (_numArgs != args.Length)
            {
                throw new ArgumentException($"Expected {_numArgs} arguments, but got {args.Length}", nameof(args));
            }

            ParamsArray<object?, object?, object?> pa;
            switch (args.Length)
            {
                case 1:
                    pa = new ParamsArray<object?, object?, object?>(args[0], null, null, 1);
                    break;
                case 2:
                    pa = new ParamsArray<object?, object?, object?>(args[0], args[1], null, 2);
                    break;
                case 3:
                    pa = new ParamsArray<object?, object?, object?>(args[0], args[1], args[2], 3);
                    break;
                default:
                    pa = new ParamsArray<object?, object?, object?>(args[0], args[1], args[2], args.AsSpan(3));
                    break;
            }

            return TryFormat(destination, out charsWritten, provider, in pa);
        }

        private bool TryFormat<T0, T1, T2>(Span<char> destination, out int charsWritten, IFormatProvider? provider, in ParamsArray<T0, T1, T2> pa)
        {
            var formatter = new ValueStringBuilder(destination);
            formatter.Format<T0, T1, T2>(provider, in pa, _segments, _literalString);
            
            if (formatter.Length > destination.Length)
            {
                // there was a reallocation, so the output didn't fit...
                charsWritten = 0;
                return false;
            }

            charsWritten = formatter.Length;
            return true;
        }

        /// <summary>
        /// Gets the number of arguments required in order to produce a string with this instance.
        /// </summary>
        public int NumArgumentsNeeded
        {
            get { return _numArgs; }
        }
    }
}
