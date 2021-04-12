// © Microsoft Corporation. All rights reserved.

// #define PUBLIC_STRINGMAKER

using System;
using System.Buffers;
using System.Text;

namespace Text
{
#if PUBLIC_STRINGMAKERX
    public
#else
    internal
#endif
    ref struct StringMaker
    {
        public const int DefaultCapacity = 128;

        private readonly bool _fixedCapacity;
#pragma warning disable SA1011 // Closing square brackets should be spaced correctly
        private char[]? _rentedBuffer;
#pragma warning restore SA1011 // Closing square brackets should be spaced correctly
        private Span<char> _chars;

        public StringMaker(Span<char> initialBuffer, bool fixedCapacity = false)
        {
            _rentedBuffer = null;
            _chars = initialBuffer;
            _fixedCapacity = fixedCapacity;
            Length = 0;
            Overflowed = false;
        }

        public StringMaker(int initialCapacity)
        {
            if (initialCapacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(initialCapacity));
            }

            _rentedBuffer = ArrayPool<char>.Shared.Rent(initialCapacity);
            _chars = _rentedBuffer;
            _fixedCapacity = false;
            Length = 0;
            Overflowed = false;
        }

        public void Dispose()
        {
            if (_rentedBuffer != null)
            {
                ArrayPool<char>.Shared.Return(_rentedBuffer);
            }

            // clear out everything to prevent accidental reuse
            this = default;
        }

        public string ExtractString()
        {
            var s = _chars.Slice(0, Length).ToString();
            Dispose();
            return s;
        }

        public ReadOnlySpan<char> ExtractSpan()
        {
            if (_rentedBuffer != null)
            {
                return ExtractString();
            }

            var s = _chars.Slice(0, Length);
            Dispose();
            return s;
        }

        internal void AppendTo(StringBuilder sb)
        {
            _ = sb.Append(_chars.Slice(0, Length));
        }

        public int Length { get; private set; }
        public bool Overflowed { get; private set; }

        public void Fill(char value, int count)
        {
            if (Length > _chars.Length - count)
            {
                if (!Expand(count))
                {
                    return;
                }
            }

            _chars.Slice(Length, count).Fill(value);
            Length += count;
        }

#if PUBLIC_STRINGMAKER
        public void Append(string? value)
        {
            if (value == null)
            {
                return;
            }

            if (Length > _chars.Length - value.Length)
            {
                if (!Expand(value.Length))
                {
                    return;
                }
            }

            value.AsSpan().CopyTo(_chars.Slice(Length));
            Length += value.Length;
        }
#endif
        public void Append(string? value, int width)
        {
            if (value == null)
            {
                Fill(' ', width);
            }
            else if (width == 0)
            {
                if (Length > _chars.Length - value.Length)
                {
                    if (!Expand(value.Length))
                    {
                        return;
                    }
                }

                value.AsSpan().CopyTo(_chars.Slice(Length));
                Length += value.Length;
            }
            else if (width > value.Length)
            {
                Fill(' ', width - value.Length);
                FinishAppend(value, 0);
            }
            else
            {
                FinishAppend(value, width);
            }
        }

        public void Append(ReadOnlySpan<char> value)
        {
            if (Length > _chars.Length - value.Length)
            {
                if (!Expand(value.Length))
                {
                    return;
                }
            }

            value.CopyTo(_chars.Slice(Length));
            Length += value.Length;
        }

        public void Append(ReadOnlySpan<char> value, int width)
        {
            if (width == 0)
            {
                if (Length > _chars.Length - value.Length)
                {
                    if (!Expand(value.Length))
                    {
                        return;
                    }
                }

                value.CopyTo(_chars.Slice(Length));
                Length += value.Length;
            }
            else if (width > value.Length)
            {
                Fill(' ', width - value.Length);
                FinishAppend(value, 0);
            }
            else
            {
                FinishAppend(value, width);
            }
        }

        public void Append(char value)
        {
            if (Length == _chars.Length)
            {
                if (!Expand(1))
                {
                    return;
                }
            }

            _chars[Length++] = value;
        }

        public void Append(char value, int width)
        {
            if (width >= -1 && width <= 1)
            {
                if (Length == _chars.Length)
                {
                    if (!Expand(1))
                    {
                        return;
                    }
                }

                _chars[Length++] = value;
            }
            else if (width > 1)
            {
                if (Length > _chars.Length - width)
                {
                    if (!Expand(width))
                    {
                        return;
                    }
                }

                _chars.Slice(Length, width - 1).Fill(' ');
                Length += width;
                _chars[Length - 1] = value;
            }
            else
            {
                width = -width;
                if (Length > _chars.Length - width)
                {
                    if (!Expand(width))
                    {
                        return;
                    }
                }

                _chars[Length++] = value;
                _chars.Slice(Length, width - 1).Fill(' ');
                Length += width - 1;
            }
        }

#if PUBLIC_STRINGMAKER
        public void Append(long value)
        {
            int charsWritten;
            while (!value.TryFormat(_chars.Slice(Length), out charsWritten, string.Empty))
            {
                if (!Expand())
                {
                    return;
                }
            }

            Length += charsWritten;
        }
#endif
        public void Append(long value, string? format, IFormatProvider? provider, int width)
        {
            int charsWritten;
            while (!value.TryFormat(_chars.Slice(Length), out charsWritten, format, provider))
            {
                if (!Expand())
                {
                    return;
                }
            }

            FinishAppend(charsWritten, width);
        }

#if PUBLIC_STRINGMAKER
        public void Append(ulong value)
        {
            int charsWritten;
            while (!value.TryFormat(_chars.Slice(Length), out charsWritten, string.Empty))
            {
                if (!Expand())
                {
                    return;
                }
            }

            Length += charsWritten;
        }
#endif

        public void Append(ulong value, string? format, IFormatProvider? provider, int width)
        {
            int charsWritten;
            while (!value.TryFormat(_chars.Slice(Length), out charsWritten, format, provider))
            {
                if (!Expand())
                {
                    return;
                }
            }

            FinishAppend(charsWritten, width);
        }

#if PUBLIC_STRINGMAKER
        public void Append(double value)
        {
            int charsWritten;
            while (!value.TryFormat(_chars.Slice(Length), out charsWritten, string.Empty))
            {
                if (!Expand())
                {
                    return;
                }
            }

            Length += charsWritten;
        }
#endif

        public void Append(double value, string? format, IFormatProvider? provider, int width)
        {
            int charsWritten;
            while (!value.TryFormat(_chars.Slice(Length), out charsWritten, format, provider))
            {
                if (!Expand())
                {
                    return;
                }
            }

            FinishAppend(charsWritten, width);
        }

#if PUBLIC_STRINGMAKER
        public void Append(bool value)
        {
            int charsWritten;
            while (!value.TryFormat(_chars.Slice(Length), out charsWritten))
            {
                if (!Expand())
                {
                    return;
                }
            }

            Length += charsWritten;
        }
#endif

        public void Append(bool value, int width)
        {
            int charsWritten;
            while (!value.TryFormat(_chars.Slice(Length), out charsWritten))
            {
                if (!Expand())
                {
                    return;
                }
            }

            FinishAppend(charsWritten, width);
        }

#if PUBLIC_STRINGMAKER
        public void Append(decimal value)
        {
            int charsWritten;
            while (!value.TryFormat(_chars.Slice(Length), out charsWritten, string.Empty))
            {
                if (!Expand())
                {
                    return;
                }
            }

            Length += charsWritten;
        }
#endif

        public void Append(decimal value, string? format, IFormatProvider? provider, int width)
        {
            int charsWritten;
            while (!value.TryFormat(_chars.Slice(Length), out charsWritten, format, provider))
            {
                if (!Expand())
                {
                    return;
                }
            }

            FinishAppend(charsWritten, width);
        }

#if PUBLIC_STRINGMAKER
        public void Append(DateTime value)
        {
            int charsWritten;
            while (!value.TryFormat(_chars.Slice(Length), out charsWritten, string.Empty))
            {
                if (!Expand())
                {
                    return;
                }
            }

            Length += charsWritten;
        }
#endif

        public void Append(DateTime value, string? format, IFormatProvider? provider, int width)
        {
            int charsWritten;
            while (!value.TryFormat(_chars.Slice(Length), out charsWritten, format, provider))
            {
                if (!Expand())
                {
                    return;
                }
            }

            FinishAppend(charsWritten, width);
        }

#if PUBLIC_STRINGMAKER
        public void Append(TimeSpan value)
        {
            int charsWritten;
            while (!value.TryFormat(_chars.Slice(Length), out charsWritten, string.Empty))
            {
                if (!Expand())
                {
                    return;
                }
            }

            Length += charsWritten;
        }
#endif

        public void Append(TimeSpan value, string? format, IFormatProvider? provider, int width)
        {
            int charsWritten;
            while (!value.TryFormat(_chars.Slice(Length), out charsWritten, format, provider))
            {
                if (!Expand())
                {
                    return;
                }
            }

            FinishAppend(charsWritten, width);
        }

#if PUBLIC_STRINGMAKER
        public void Append<T>(T value)
            where T : ISpanFormattable
        {
            int charsWritten;
            while (!value.TryFormat(_chars.Slice(Length), out charsWritten, string.Empty, null))
            {
                if (!Expand())
                {
                    return;
                }
            }

            Length += charsWritten;
        }
#endif

        public void Append<T>(T value, string? format, IFormatProvider? provider, int width)
            where T : ISpanFormattable
        {
            int charsWritten;
            while (!value.TryFormat(_chars.Slice(Length), out charsWritten, format, provider))
            {
                if (!Expand())
                {
                    return;
                }
            }

            FinishAppend(charsWritten, width);
        }

#if PUBLIC_STRINGMAKER
        public void Append(IFormattable value)
        {
            FinishAppend(value.ToString(string.Empty, null), 0);
        }
#endif

        public void Append(IFormattable value, string? format, IFormatProvider? provider, int width)
        {
            FinishAppend(value.ToString(format, provider), width);
        }

#if PUBLIC_STRINGMAKER
        public void Append(object? value)
        {
            if (value == null)
            {
                FinishAppend(string.Empty, 0);
                return;
            }

            FinishAppend(value.ToString(), 0);
        }
#endif

        public void Append(object? value, int width)
        {
            if (value == null)
            {
                FinishAppend(string.Empty, width);
                return;
            }

            FinishAppend(value.ToString(), width);
        }

        private void FinishAppend(int charsWritten, int width)
        {
            Length += charsWritten;

            var leftAlign = false;
            if (width < 0)
            {
                width = -width;
                leftAlign = true;
            }

            int padding = width - charsWritten;
            if (padding > 0)
            {
                if (Length > _chars.Length - padding)
                {
                    if (!Expand(padding))
                    {
                        return;
                    }
                }

                if (leftAlign)
                {
                    _chars.Slice(Length, padding).Fill(' ');
                }
                else
                {
                    int start = Length - charsWritten;
                    _chars.Slice(start, charsWritten).CopyTo(_chars.Slice(start + padding));
                    _chars.Slice(start, padding).Fill(' ');
                }

                Length += padding;
            }
        }

        private void FinishAppend(ReadOnlySpan<char> result, int width)
        {
            var leftAlign = false;
            if (width < 0)
            {
                width = -width;
                leftAlign = true;
            }

            int padding = width - result.Length;
            int extra = result.Length;
            if (padding > 0)
            {
                extra += padding;
            }

            if (Length > _chars.Length - extra)
            {
                if (!Expand(extra))
                {
                    return;
                }
            }

            if (padding > 0)
            {
                if (leftAlign)
                {
                    result.CopyTo(_chars.Slice(Length));
                    _chars.Slice(Length + result.Length, padding).Fill(' ');
                }
                else
                {
                    _chars.Slice(Length, padding).Fill(' ');
                    result.CopyTo(_chars.Slice(Length + padding));
                }
            }
            else
            {
                result.CopyTo(_chars.Slice(Length));
            }

            Length += extra;
        }

        private bool Expand(int neededCapacity = 0)
        {
            if (_fixedCapacity)
            {
                Overflowed = true;
                return false;
            }

            if (neededCapacity == 0)
            {
                neededCapacity = DefaultCapacity;
            }

            int newCapacity = _chars.Length + neededCapacity;

            // allocate a new array and copy the existing data to it
            var a = ArrayPool<char>.Shared.Rent(newCapacity);
            _chars.Slice(0, Length).CopyTo(a);

            if (_rentedBuffer != null)
            {
                ArrayPool<char>.Shared.Return(_rentedBuffer);
            }

            _rentedBuffer = a;
            _chars = a;
            return true;
        }
    }
}
