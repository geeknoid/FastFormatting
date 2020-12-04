// © Microsoft Corporation. All rights reserved.

namespace System.Text
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Holds the pre-parsed representation of a composite format string.
    /// </summary>
    /// <remarks>
    /// For code that needs to repeatedly perform string formatting against the same composite format
    /// string, it is considerably more efficient to parse and validate the composite format string only
    /// once and then quickly format the inputs accordingly.
    /// </remarks>
    public partial class StringFormatter
    {
        readonly FormatterSegment[] _segments;
        readonly string _literalString;
        readonly int _numArgs;

        /// <summary>
        /// Parses a composite format string into an efficient form for later use.
        /// </summary>
        public StringFormatter(string compositeFormat)
        {
            if (compositeFormat == null)
            {
                throw new ArgumentNullException(nameof(compositeFormat));
            }

            int pos = 0;
            int len = compositeFormat.Length;
            char ch = '\0';
            StringBuilder? tmp = null;
            var segments = new List<FormatterSegment>();
            int argCount = 0;
            StringBuilder literal = new StringBuilder(compositeFormat.Length);

            for (; ; )
            {
                int segStart = literal.Length;
                while (pos < len)
                {
                    ch = compositeFormat[pos];

                    pos++;
                    if (ch == '}')
                    {
                        if (pos < len && compositeFormat[pos] == '}')
                        {
                            // double }, treat as escape sequence
                            pos++;
                        }
                        else
                        {
                            // dangling }
                            // Release.Fail("Dangling } in format string.");
                        }
                    }
                    else if (ch == '{')
                    {
                        if (pos < len && compositeFormat[pos] == '{')
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

                int total = literal.Length - segStart;
                if (total > 0)
                {
                    while (total > 0)
                    {
                        int num = total;
                        if (num > byte.MaxValue)
                        {
                            num = byte.MaxValue;
                        }

                        segments.Add(FormatterSegment.Literal((byte)num));
                        total -= num;
                    }
                    continue;
                }

                if (pos == len)
                {
                    // done
                    _literalString = literal.ToString();
                    _numArgs = argCount;

                    FormatterSegment[] a = new FormatterSegment[segments.Count];
                    for (int i = 0; i < segments.Count; i++)
                    {
                        a[i] = segments[i];
                    }
                    _segments = a;
                    return;
                }

                pos++;
                if (pos == len || (ch = compositeFormat[pos]) < '0' || ch > '9')
                {
                    // we need an argument index
                    //                    Release.Fail("Missing argument index in format string.");
                }

                // extract the argument index
                int index = 0;
                do
                {
                    index = (index * 10) + (ch - '0');
                    pos++;

                    // make sure we get a suitable end to the argument index
                    //                  Release.Assert(pos != len, "Invalid argument index.");

                    ch = compositeFormat[pos];
                } while (ch >= '0' && ch <= '9');

                if (index >= argCount)
                {
                    // new max arg count
                    argCount = index + 1;
                }

                // skip whitespace
                while (pos < len && (ch = compositeFormat[pos]) == ' ')
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
                    while (pos < len && compositeFormat[pos] == ' ')
                    {
                        pos++;
                    }

                    // did we run out of steam
                    //                Release.Assert(pos != len, "Invalid format specification.");

                    bool dynamicWidth = false;
                    ch = compositeFormat[pos];
                    if (ch == '-')
                    {
                        leftJustify = true;
                        pos++;

                        // did we run out of steam?
                        //                  Release.Assert(pos != len, "Invalid format specification.");

                        ch = compositeFormat[pos];
                    }
                    else if (ch == '*')
                    {
                        dynamicWidth = true;
                        pos++;

                        // did we run out of steam?
                        //                Release.Assert(pos != len, "Invalid format specification.");

                        ch = compositeFormat[pos];
                    }

                    //          Release.Assert(ch >= '0' && ch <= '9', "Invalid character in field width specification.");

                    int val = 0;
                    do
                    {
                        val = (val * 10) + (ch - '0');
                        pos++;

                        // did we run out of steam?
                        //            Release.Assert(pos != len, "Invalid format specification.");

                        // did we get a number that's too big?
                        if (!dynamicWidth)
                        {
                            //Release.Assert(val < MaxWidth, "Field width value exceeds limit.");
                        }

                        ch = compositeFormat[pos];
                    } while (ch >= '0' && ch <= '9');

                    if (dynamicWidth)
                    {
                        // indicates the width is in fact an arg index
                        width = -val;

                        if (val >= argCount)
                        {
                            // new arg count
                            argCount = val + 1;
                        }
                    }
                    else
                    {
                        width = val;
                    }
                }

                // skip whitespace
                while (pos < len && (ch = compositeFormat[pos]) == ' ')
                {
                    pos++;
                }

                // parse the optional custom format string

                if (ch == ':')
                {
                    pos++;

                    if (tmp == null)
                    {
                        tmp = new StringBuilder();
                    }
                    else
                    {
                        tmp.Clear();
                    }

                    for (; ; )
                    {

                        //          Release.Assert(pos != len, "Invalid format specification.");

                        ch = compositeFormat[pos];
                        pos++;
                        if (ch == '{')
                        {
                            if (pos < len && compositeFormat[pos] == '{')
                            {
                                // double {, an escape sequence
                                pos++;
                            }
                            else
                            {
                                //                Release.Fail("Nested { in format specification.");
                            }
                        }
                        else if (ch == '}')
                        {
                            if (pos < len && compositeFormat[pos] == '}')
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
                        else if (ch == '*')
                        {
                            // argument list reference

                            tmp.Append('*');
                            int val = 0;
                            for (; ; )
                            {
                                ch = compositeFormat[pos];
                                if (ch < '0' || ch > '9')
                                {
                                    break;
                                }

                                val = (val * 10) + (ch - '0');
                                pos++;

                                // did we run out of steam?
                                //             Release.Assert(pos != len, "Invalid format specification.");

                                tmp.Append(ch);
                            }

                            if (val >= argCount)
                            {
                                // new arg count
                                argCount = val + 1;
                            }

                            continue;
                        }

                        tmp.Append(ch);
                    }
                }

                //Release.Assert(ch == '}', "Unterminated format specification.");

                // skip over the closing }
                pos++;

                // process the optional format string
                string? fmtStr = null;
                if ((tmp != null) && (tmp.Length > 0))
                {
                    fmtStr = tmp.ToString();
                    tmp.Clear();
                }

                //                Release.Assert(argCount <= byte.MaxValue, "Must have less than 256 arguments");

                segments.Add(FormatterSegment.Full(fmtStr, (short)width, leftJustify, (byte)index));
            }
        }

        // TODO: format to a span?
        // TODO: make it so a Segment includes a literal preamble and then a format sequence
        // TODO: API docs

        private const int MaxStackAlloc = 128;  // = 256 bytes

        public string Format<T>(IFormatProvider? provider, T arg)
        {
            if (_numArgs != 1)
            {
                throw new ArgumentException($"Expected {_numArgs} arguments, but got 1");
            }

            var pa = new ParamsArray<T, bool, bool>(arg, false, false);
            return Format(provider, in pa);
        }

        public string Format<T0, T1>(IFormatProvider? provider, T0 arg0, T1 arg1)
        {
            if (_numArgs != 2)
            {
                throw new ArgumentException($"Expected {_numArgs} arguments, but got 2");
            }

            var pa = new ParamsArray<T0, T1, bool>(arg0, arg1, false);
            return Format(provider, in pa);
        }

        public string Format<T0, T1, T2>(IFormatProvider? provider, T0? arg0, T1? arg1, T2? arg2)
        {
            if (_numArgs != 3)
            {
                throw new ArgumentException($"Expected {_numArgs} arguments, but got 3");
            }

            var pa = new ParamsArray<T0, T1, T2>(arg0, arg1, arg2);
            return Format(provider, in pa);
        }

        public string Format<T0, T1, T2>(IFormatProvider? provider, T0 arg0, T1 arg1, T2 arg2, params object?[]? args)
        {
#pragma warning disable CA1508 // Avoid dead conditional code
            if (args == null || args.Length == 0)
#pragma warning restore CA1508 // Avoid dead conditional code
            {
                if (_numArgs != 0)
                {
                    throw new ArgumentException($"Expected {_numArgs} arguments, but got none", nameof(args));
                }
                return _literalString;
            }

            if (args.Length + 3 != _numArgs)
            {
                throw new ArgumentException($"Expected {_numArgs} arguments, but got {args.Length + 3}", nameof(args));
            }

            var pa = new ParamsArray<T0, T1, T2>(arg0, arg1, arg2, args);
            return Format(provider, in pa);
        }

        public string Format(IFormatProvider? provider, params object?[]? args)
        {
#pragma warning disable CA1508 // Avoid dead conditional code
            if (args == null || args.Length == 0)
#pragma warning restore CA1508 // Avoid dead conditional code
            {
                if (_numArgs != 0)
                {
                    throw new ArgumentException($"Expected {_numArgs} arguments, but got none", nameof(args));
                }
                return _literalString;
            }

            if (args.Length != _numArgs)
            {
                throw new ArgumentException($"Expected {_numArgs} arguments, but got {args.Length}", nameof(args));
            }

            object? arg0 = null;
            object? arg1 = null;
            object? arg2 = null;

            if (args.Length >= 3)
            {
                arg0 = args[0];
                arg1 = args[1];
                arg2 = args[2];
            }
            else if (args.Length == 2)
            {
                arg0 = args[0];
                arg1 = args[1];
            }
            else if (args.Length == 1)
            {
                arg0 = args[0];
            }

            // TODO: Reallocate and copy array
            args = Array.Empty<object>();

            var pa = new ParamsArray<object, object, object>(arg0, arg1, arg2, args);
            return Format(provider, in pa);
        }

        private string Format<T0, T1, T2>(IFormatProvider? provider, in ParamsArray<T0, T1, T2> pa)
        {
            // make a guesstimate at the size of the buffer we need for output
            int estimatedSize = _literalString.Length + _numArgs * 16;
            if (_numArgs >= 1)
            {
                var str = pa.Arg0 as string;
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

        /// <summary>
        /// Gets the number of arguments required in order to produce a string with this instance.
        /// </summary>
        public int NumArgumentsNeeded
        {
            get { return _numArgs; }
        }
    }
}
