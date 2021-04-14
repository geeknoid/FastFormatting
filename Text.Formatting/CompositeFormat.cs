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
    public readonly partial struct CompositeFormat
    {
        internal const int MaxStackAlloc = 128;  // = 256 bytes

        private readonly Segment[] _segments;     // info on the different chunks to process

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeFormat"/> struct.
        /// </summary>
        /// <param name="format">A classic .NET format string as used with <see cref="string.Format(string,object?)"  />.</param>
        /// <remarks>
        /// Parses a composite format string into an efficient form for later use.
        /// </remarks>
        public CompositeFormat(ReadOnlySpan<char> format)
        {
            (_segments, NumArgumentsNeeded, LiteralString) = Parse(format, null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeFormat"/> struct.
        /// </summary>
        /// <param name="format">A template-based .NET format string as used with <c>LoggerMessage.Define</c>.</param>
        /// <param name="templates">Holds the named templates discovered in the format string.</param>
        /// <remarks>
        /// Parses a composite format string into an efficient form for later use.
        /// </remarks>
        public CompositeFormat(ReadOnlySpan<char> format, out IList<string> templates)
        {
            var l = new List<string>();
            (_segments, NumArgumentsNeeded, LiteralString) = Parse(format, l);
            templates = l;
        }

        internal static int EstimateArgSize<T>(T arg)
        {
            var str = arg as string;
            if (str != null)
            {
                return str.Length;
            }

            return 8;
        }

        internal static int EstimateArgSize(object?[]? args)
        {
            int total = 0;

            if (args != null)
            {
                foreach (var arg in args)
                {
                    if (arg is string str)
                    {
                        total += str.Length;
                    }
                }
            }

            return total;
        }

        internal void CheckNumArgs(int explicitCount, object?[]? args)
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

        private static void AppendArg<T>(ref StringMaker sm, T arg, string argFormat, IFormatProvider? provider, int argWidth)
        {
            switch (arg)
            {
                case int a:
                    sm.Append(a, argFormat, provider, argWidth);
                    break;

                case long a:
                    sm.Append(a, argFormat, provider, argWidth);
                    break;

                case string a:
                    sm.Append(a, argWidth);
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

        private static bool ValidTemplateNameChar(char ch, bool first)
        {
            if (first)
            {
                return char.IsLetter(ch) || ch == '_';
            }

            return char.IsLetterOrDigit(ch) || ch == '_';
        }

        private static (Segment[] segments, int numArgsRequired, string literalString) Parse(ReadOnlySpan<char> format, List<string>? templates)
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

                    return (segments.ToArray(), numArgs, literal.ExtractString());
                }

                // extract the argument index
                var argIndex = 0;
                if (templates == null)
                {
                    // classic composite format string

                    pos++;
                    if (pos == len || (ch = format[pos]) < '0' || ch > '9')
                    {
                        // we need an argument index
                        throw new ArgumentException($"Missing argument index in format string at position {pos}", nameof(format));
                    }

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
                }
                else
                {
                    // template-based format string

                    pos++;
                    if (pos == len)
                    {
                        // we need a template name
                        throw new ArgumentException($"Missing template name in format string at position {pos}", nameof(format));
                    }

                    ch = format[pos];
                    if (!ValidTemplateNameChar(ch, true))
                    {
                        // we need a template name
                        throw new ArgumentException($"Missing template name in format string at position {pos}", nameof(format));
                    }

                    // extract the template name
                    var start = pos;
                    do
                    {
                        pos++;

                        // make sure we get a suitable end
                        if (pos == len)
                        {
                            throw new ArgumentException($"Invalid template name in format string at position {pos}", nameof(format));
                        }

                        ch = format[pos];
                    }
                    while (ValidTemplateNameChar(ch, false));

                    // get an argument index for the given template
                    var template = format.Slice(start, pos - start).ToString();
                    argIndex = templates.IndexOf(template);
                    if (argIndex < 0)
                    {
                        templates.Add(template);
                        argIndex = numArgs;
                    }
                }

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

        private void CoreFmt<T0, T1, T2>(ref StringMaker sm, IFormatProvider? provider, T0 arg0, T1 arg1, T2 arg2, ReadOnlySpan<object?> args)
        {
            var literalIndex = 0;
            foreach (var segment in _segments)
            {
                int literalCount = segment.LiteralCount;
                if (literalCount > 0)
                {
                    // the segment has some literal text
                    sm.Append(LiteralString.AsSpan(literalIndex, literalCount));
                    literalIndex += literalCount;
                }

                var argIndex = segment.ArgIndex;
                if (argIndex >= 0)
                {
                    // the segment has an arg to format
                    switch (argIndex)
                    {
                        case 0:
                            AppendArg(ref sm, arg0, segment.ArgFormat, provider, segment.ArgWidth);
                            break;

                        case 1:
                            AppendArg(ref sm, arg1, segment.ArgFormat, provider, segment.ArgWidth);
                            break;

                        case 2:
                            AppendArg(ref sm, arg2, segment.ArgFormat, provider, segment.ArgWidth);
                            break;

                        default:
                            AppendArg(ref sm, args[argIndex - 3], segment.ArgFormat, provider, segment.ArgWidth);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the number of arguments required in order to produce a string with this instance.
        /// </summary>
        public int NumArgumentsNeeded { get; }

        /// <summary>
        /// Gets all literal text to be inserted into the output.
        /// </summary>
        /// <remarks>
        /// In the case where the format string doesn't contain any formatting
        /// sequence, this literal is the string to produce when formatting.
        /// </remarks>
        private readonly string LiteralString { get; }
    }
}
