// © Microsoft Corporation. All rights reserved.

namespace System.Text
{
    using System;
    using System.Runtime.CompilerServices;

    internal ref partial struct ValueStringBuilder
    {
        public void Format<T0, T1, T2>(IFormatProvider? provider, in ParamsArray<T0, T1, T2> pa, FormatterSegment[] segments, string literalString)
        {
            int literalIndex = 0;
            foreach (var segment in segments)
            {
                int literalCount = segment.LiteralCount;
                if (literalCount > 0)
                {
                    // the segment has some literal text
                    var substring = literalString.AsSpan(literalIndex, literalCount);
                    Append(substring);
                    literalIndex += literalCount;
                }

                var argIndex = segment.ArgIndex;
                if (argIndex >= 0)
                {
                    // the segment has an arg to format
                    switch (argIndex)
                    {
                        case 0:
                            HandleArg(pa.Arg0, segment.Format, segment.Width, provider);
                            break;

                        case 1:
                            HandleArg(pa.Arg1, segment.Format, segment.Width, provider);
                            break;

                        case 2:
                            HandleArg(pa.Arg2, segment.Format, segment.Width, provider);
                            break;

                        default:
                            HandleReferenceArg(pa.Args[argIndex - 3], segment.Format, segment.Width, provider);
                            break;
                    }
                }
            }
        }

        private void HandleArg<T>(T arg, string argFormat, int argWidth, IFormatProvider? provider)
        {
            switch (arg)
            {
                case int a:
                    FinishArg(Append(a, argFormat, provider), argWidth, false);
                    break;

                case long a:
                    FinishArg(Append(a, argFormat, provider), argWidth, false);
                    break;

                case double a:
                    FinishArg(Append(a, argFormat, provider), argWidth, false);
                    break;

                case float a:
                    FinishArg(Append(a, argFormat, provider), argWidth, false);
                    break;

                case uint a:
                    FinishArg(Append(a, argFormat, provider), argWidth, false);
                    break;

                case ulong a:
                    FinishArg(Append(a, argFormat, provider), argWidth, false);
                    break;

                case short a:
                    FinishArg(Append(a, argFormat, provider), argWidth, false);
                    break;

                case ushort a:
                    FinishArg(Append(a, argFormat, provider), argWidth, false);
                    break;

                case byte a:
                    FinishArg(Append(a, argFormat, provider), argWidth, false);
                    break;

                case sbyte a:
                    FinishArg(Append(a, argFormat, provider), argWidth, false);
                    break;

                case bool a:
                    FinishArg(Append(a), argWidth, false);
                    break;

                case DateTime a:
                    FinishArg(Append(a, argFormat, provider), argWidth, false);
                    break;

                case TimeSpan a:
                    FinishArg(Append(a, argFormat, provider), argWidth, false);
                    break;

                case Guid a:
                    FinishArg(Append(a, argFormat), argWidth, false);
                    break;

                case decimal a:
                    FinishArg(Append(a, argFormat, provider), argWidth, false);
                    break;

                default:
                    HandleReferenceArg(arg, argFormat, argWidth, provider);
                    break;
            }
        }

        private void HandleReferenceArg(object? arg, string argFormat, int argWidth, IFormatProvider? provider)
        {
            switch (arg)
            {
                case ISpanFormattable a:
                    FinishArg(Append(a, argFormat, provider), argWidth, false);
                    break;

                case IFormattable a:
                    FinishArg(a.ToString(argFormat, provider), argWidth, true);
                    break;

                case object a:
                    FinishArg(a.ToString(), argWidth, true);
                    break;

                default:
                    // when arg == null
                    FinishArg(Array.Empty<char>(), argWidth, false);
                    break;
            }
        }

        /// <summary>
        /// Perform the final padding and insertion if needed.
        /// </summary>
        /// <remarks>
        /// If appendResult is false, it means the result is already present in the
        /// main output buffer. Otherwise, the result is in a distinct span and needs
        /// to be copied into the output buffer.
        /// </remarks>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void FinishArg(ReadOnlySpan<char> result, int width, bool appendResult)
        {
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
                    if (appendResult)
                    {
                        Append(result);
                    }
                    Append(' ', padding);
                }
                else
                {
                    if (appendResult)
                    {
                        Append(' ', padding);
                        Append(result);
                    }
                    else
                    {
                        Insert(Length - result.Length, ' ', padding);
                    }
                }
            }
            else if (appendResult)
            {
                Append(result);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private ReadOnlySpan<char> Append(long value, ReadOnlySpan<char> format, IFormatProvider? provider)
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
        private ReadOnlySpan<char> Append(ulong value, ReadOnlySpan<char> format, IFormatProvider? provider)
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
        private ReadOnlySpan<char> Append(bool value)
        {
            var s = _chars.Slice(_pos);
            int charsWritten;

            while (!value.TryFormat(s, out charsWritten))
            {
                EnsureCapacity(s.Length * 2);
                s = _chars.Slice(_pos);
            }

            s = _chars.Slice(_pos, charsWritten);
            _pos += charsWritten;
            return s;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private ReadOnlySpan<char> Append(DateTime value, ReadOnlySpan<char> format, IFormatProvider? provider)
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
        private ReadOnlySpan<char> Append(TimeSpan value, ReadOnlySpan<char> format, IFormatProvider? provider)
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
        private ReadOnlySpan<char> Append(decimal value, ReadOnlySpan<char> format, IFormatProvider? provider)
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
        private ReadOnlySpan<char> Append(Guid value, ReadOnlySpan<char> format)
        {
            var s = _chars.Slice(_pos);
            int charsWritten;

            while (!value.TryFormat(s, out charsWritten, format))
            {
                EnsureCapacity(s.Length * 2);
                s = _chars.Slice(_pos);
            }

            s = _chars.Slice(_pos, charsWritten);
            _pos += charsWritten;
            return s;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private ReadOnlySpan<char> Append(double value, ReadOnlySpan<char> format, IFormatProvider? provider)
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
