// � Microsoft Corporation. All rights reserved.

namespace System.Text
{
    using System;
    using System.Runtime.CompilerServices;

    internal ref partial struct ValueStringBuilder
    {
        public void Format<T0, T1, T2>(IFormatProvider? provider, in ParamsArray<T0, T1, T2> pa, FormatterSegment[] segments, string literalString)
        {
            int literalIndex = 0;
            for (int i = 0; i < segments.Length; i++)
            {
                int literalCount = segments[i].LiteralCount;
                if (literalCount > 0)
                {
                    // the segment is a literal
                    var substring = literalString.AsSpan(literalIndex, literalCount);
                    Append(substring);
                    literalIndex += literalCount;
                }

                var argIndex = segments[i].ArgIndex;
                if (argIndex >= 0)
                {
                    string argFormat = segments[i].Format;
                    ReadOnlySpan<char> result = default;
                    bool freshSpan = false;

                    switch (argIndex)
                    {
                        case 0:
                            result = HandleArg(pa.Arg0, argFormat, provider, out freshSpan);
                            break;

                        case 1:
                            result = HandleArg(pa.Arg1, argFormat, provider, out freshSpan);
                            break;

                        case 2:
                            result = HandleArg(pa.Arg2, argFormat, provider, out freshSpan);
                            break;

                        default:
                            result = HandleArg(pa.Args[argIndex - 3], argFormat, provider, out freshSpan);
                            break;
                    }

                    ApplyPadding(in segments[i], result, freshSpan);
                }
            }
        }

        private ReadOnlySpan<char> HandleArg<T>(T arg, string argFormat, IFormatProvider? provider, out bool freshSpan)
        {
            switch (arg)
            {
                case int a:
                    freshSpan = false;
                    return Append(a, argFormat, provider);

                case ISpanFormattable a:
                    freshSpan = false;
                    return Append(a, argFormat, provider);

                case IFormattable a:
                    freshSpan = true;
                    return a.ToString(argFormat, provider);

                case object a:
                    freshSpan = true;
                    return a.ToString();

                default:
                    freshSpan = false;
                    return Array.Empty<char>();
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void ApplyPadding(in FormatterSegment segment, ReadOnlySpan<char> result, bool freshSpan)
        {
            var width = (int)segment.Width;
            var leftJustify = true;

            if (width < 0)
            {
                width = -width;
                leftJustify = false;
            }

            int padding = (width - result.Length);

            if (padding > 0)
            {
                if (leftJustify)
                {
                    if (freshSpan)
                    {
                        Append(result);
                    }
                    Append(' ', padding);
                }
                else if (freshSpan)
                {
                    Append(' ', padding);
                    Append(result);
                }
                else
                {
                    Insert(Length - result.Length, ' ', padding);
                }
            }
            else if (freshSpan)
            {
                Append(result);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private ReadOnlySpan<char> Append(int value, ReadOnlySpan<char> format, IFormatProvider? provider)
        {
            var s = _chars.Slice(_pos);
            int charsWritten;

            while (!value.TryFormat(s, out charsWritten, format, provider))
            {
                EnsureCapacity(s.Length * 2);
                s = _chars.Slice(_pos);
            }

            s = _chars.Slice(_pos, charsWritten);
            _pos += charsWritten;
            return s;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private ReadOnlySpan<char> Append(ISpanFormattable value, ReadOnlySpan<char> format, IFormatProvider? provider)
        {
            var s = _chars.Slice(_pos);
            int charsWritten;

            while (!value.TryFormat(s, out charsWritten, format, provider))
            {
                EnsureCapacity(s.Length * 2);
                s = _chars.Slice(_pos);
            }

            s = _chars.Slice(_pos, charsWritten);
            _pos += charsWritten;
            return s;
        }
    }
}
