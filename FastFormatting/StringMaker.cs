// © Microsoft Corporation. All rights reserved.

namespace FastFormatting
{
    using System;
    using System.Buffers;
    using System.ComponentModel;

    public ref partial struct StringMaker
    {
        public const int DefaultCapacity = 128;

        private char[]? _array;
        private Span<char> _chars;
        private int _length;
        private bool _blockExpansion;
        private bool _overflowed;

        public StringMaker(Span<char> initialBuffer, bool blockExpansion = false)
        {
            _array = null;
            _chars = initialBuffer;
            _length = 0;
            _blockExpansion = blockExpansion;
            _overflowed = false;
        }

        public StringMaker(int initialCapacity)
        {
            if (initialCapacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(initialCapacity));
            }

            _array = ArrayPool<char>.Shared.Rent(initialCapacity);
            _chars = _array;
            _length = 0;
            _blockExpansion = false;
            _overflowed = false;
        }

        public void Dispose()
        {
            if (_array != null)
            {
                ArrayPool<char>.Shared.Return(_array);
            }

            // clear out everything to prevent accidental reuse
            this = default;
        }

        public string ExtractString()
        {
            var s = _chars.Slice(0, _length).ToString();
            Dispose();
            return s;
        }

        public ReadOnlySpan<char> ExtractSpan()
        {
            if (_array != null)
            {
                return ExtractString();
            }

            var s = _chars.Slice(_length);
            Dispose();
            return s;
        }

        public int Length => _length;
        public bool Overflowed => _overflowed;

        public void Fill(char value, int count)
        {
            if (_length > _chars.Length - count)
            {
                if (!Expand(count)) return;
            }

            _chars.Slice(_length, count).Fill(value);
            _length += count;
        }

        public void Append(string? value)
        {
            if (value == null)
            {
                return;
            }

            if (_length > _chars.Length - value.Length)
            {
                if (!Expand(value.Length)) return;
            }

            value.AsSpan().CopyTo(_chars.Slice(_length));
            _length += value.Length;
        }

        public void Append(string? value, int width)
        {
            if (value == null)
            {
                Fill(' ', width);
            }
            else if (width == 0)
            {
                if (_length > _chars.Length - value.Length)
                {
                    if (!Expand(value.Length)) return;
                }

                value.AsSpan().CopyTo(_chars.Slice(_length));
                _length += value.Length;
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
            if (_length > _chars.Length - value.Length)
            {
                if (!Expand(value.Length)) return;
            }

            value.CopyTo(_chars.Slice(_length));
            _length += value.Length;
        }

        public void Append(ReadOnlySpan<char> value, int width)
        {
            if (_length > _chars.Length - value.Length)
            {
                if (!Expand(value.Length)) return;
            }

            if (width == 0)
            {
                if (_length > _chars.Length - value.Length)
                {
                    if (!Expand(value.Length)) return;
                }

                value.CopyTo(_chars.Slice(_length));
                _length += value.Length;
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
            if (_length == _chars.Length)
            {
                if (!Expand(1)) return;
            }

            _chars[_length++] = value;
        }

        public void Append(char value, int width)
        {
            if (width >= -1 && width <= 1)
            {
                if (_length == _chars.Length)
                {
                    if (!Expand(1)) return;
                }

                _chars[_length++] = value;
            }
            else if (width > 1)
            {
                if (_length > _chars.Length - width)
                {
                    if (!Expand(width)) return;
                }

                _chars.Slice(_length, width - 1).Fill(' ');
                _length += width;
                _chars[_length - 1] = value;
            }
            else
            {
                width = -width;
                if (_length > _chars.Length - width)
                {
                    if (!Expand(width)) return;
                }

                _chars[_length++] = value;
                _chars.Slice(_length, width - 1).Fill(' ');
                _length += width - 1;
            }
        }

        public void Append(long value)
        {
            int charsWritten;
            while (!value.TryFormat(_chars.Slice(_length), out charsWritten, string.Empty, null))
            {
                if (!Expand()) return;
            }

            _length += charsWritten;
        }

        public void Append(long value, string format, IFormatProvider? provider, int width)
        {
            int charsWritten;
            while (!value.TryFormat(_chars.Slice(_length), out charsWritten, format, provider))
            {
                if (!Expand()) return;
            }

            FinishAppend(charsWritten, width);
        }

        public void Append(ulong value)
        {
            int charsWritten;
            while (!value.TryFormat(_chars.Slice(_length), out charsWritten, string.Empty, null))
            {
                if (!Expand()) return;
            }

            _length += charsWritten;
        }

        public void Append(ulong value, string format, IFormatProvider? provider, int width)
        {
            int charsWritten;
            while (!value.TryFormat(_chars.Slice(_length), out charsWritten, format, provider))
            {
                if (!Expand()) return;
            }

            FinishAppend(charsWritten, width);
        }

        public void Append(double value)
        {
            int charsWritten;
            while (!value.TryFormat(_chars.Slice(_length), out charsWritten, string.Empty, null))
            {
                if (!Expand()) return;
            }

            _length += charsWritten;
        }

        public void Append(double value, string format, IFormatProvider? provider, int width)
        {
            int charsWritten;
            while (!value.TryFormat(_chars.Slice(_length), out charsWritten, format, provider))
            {
                if (!Expand()) return;
            }

            FinishAppend(charsWritten, width);
        }

        public void Append(bool value)
        {
            int charsWritten;
            while (!value.TryFormat(_chars.Slice(_length), out charsWritten))
            {
                if (!Expand()) return;
            }

            _length += charsWritten;
        }

        public void Append(bool value, int width)
        {
            int charsWritten;
            while (!value.TryFormat(_chars.Slice(_length), out charsWritten))
            {
                if (!Expand()) return;
            }

            FinishAppend(charsWritten, width);
        }

        public void Append(decimal value)
        {
            int charsWritten;
            while (!value.TryFormat(_chars.Slice(_length), out charsWritten, string.Empty, null))
            {
                if (!Expand()) return;
            }

            _length += charsWritten;
        }

        public void Append(decimal value, string format, IFormatProvider? provider, int width)
        {
            int charsWritten;
            while (!value.TryFormat(_chars.Slice(_length), out charsWritten, format, provider))
            {
                if (!Expand()) return;
            }

            FinishAppend(charsWritten, width);
        }

        public void Append(DateTime value)
        {
            int charsWritten;
            while (!value.TryFormat(_chars.Slice(_length), out charsWritten, string.Empty, null))
            {
                if (!Expand()) return;
            }

            _length += charsWritten;
        }

        public void Append(DateTime value, string format, IFormatProvider? provider, int width)
        {
            int charsWritten;
            while (!value.TryFormat(_chars.Slice(_length), out charsWritten, format, provider))
            {
                if (!Expand()) return;
            }

            FinishAppend(charsWritten, width);
        }

        public void Append(TimeSpan value)
        {
            int charsWritten;
            while (!value.TryFormat(_chars.Slice(_length), out charsWritten, string.Empty, null))
            {
                if (!Expand()) return;
            }

            _length += charsWritten;
        }

        public void Append(TimeSpan value, string format, IFormatProvider? provider, int width)
        {
            int charsWritten;
            while (!value.TryFormat(_chars.Slice(_length), out charsWritten, format, provider))
            {
                if (!Expand()) return;
            }

            FinishAppend(charsWritten, width);
        }

        public void Append<T>(T value) where T : ISpanFormattable
        {
            int charsWritten;
            while (!value.TryFormat(_chars.Slice(_length), out charsWritten, string.Empty, null))
            {
                if (!Expand()) return;
            }

            _length += charsWritten;
        }

        public void Append<T>(T value, string format, IFormatProvider? provider, int width) where T : ISpanFormattable
        {
            int charsWritten;
            while (!value.TryFormat(_chars.Slice(_length), out charsWritten, format, provider))
            {
                if (!Expand()) return;
            }

            FinishAppend(charsWritten, width);
        }

        public void Append(IFormattable value)
        {
            FinishAppend(value.ToString(string.Empty, null), 0);
        }

        public void Append(IFormattable value, string format, IFormatProvider? provider, int width)
        {
            FinishAppend(value.ToString(format, provider), width);
        }

        public void Append(object? value)
        {
            if (value == null)
            {
                FinishAppend(string.Empty, 0);
                return;
            }

            FinishAppend(value.ToString(), 0);
        }

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
            _length += charsWritten;

            var leftAlign = false;
            if (width < 0)
            {
                width = -width;
                leftAlign = true;
            }

            int padding = (width - charsWritten);
            if (padding > 0)
            {
                if (_length > _chars.Length - padding)
                {
                    if (!Expand(padding)) return;
                }

                if (leftAlign)
                {
                    _chars.Slice(_length, padding).Fill(' ');
                }
                else
                {
                    int start = _length - charsWritten;
                    _chars.Slice(start, charsWritten).CopyTo(_chars.Slice(start + padding));
                    _chars.Slice(start, padding).Fill(' ');
                }

                _length += padding;
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

            int padding = (width - result.Length);
            int extra = result.Length;
            if (padding > 0)
            {
                extra += padding;
            }

            if (_length > _chars.Length - extra)
            {
                if (!Expand(extra)) return;
            }

            if (padding > 0)
            {
                if (leftAlign)
                {
                    result.CopyTo(_chars.Slice(_length));
                    _chars.Slice(_length + result.Length, padding).Fill(' ');
                }
                else
                {
                    _chars.Slice(_length, padding).Fill(' ');
                    result.CopyTo(_chars.Slice(_length + padding));
                }
            }
            else
            {
                result.CopyTo(_chars.Slice(_length));
            }

            _length += extra;
        }

        private bool Expand(int neededCapacity = 0)
        {
            if (_blockExpansion)
            {
                _overflowed = true;
                return false;
            }

            if (neededCapacity == 0)
            {
                neededCapacity = DefaultCapacity;
            }

            int newCapacity = _chars.Length + neededCapacity;

            // allocate a new array and copy the existing data to it
            var a = ArrayPool<char>.Shared.Rent(newCapacity);
            _chars.Slice(0, _length).CopyTo(a);

            if (_array != null)
            {
                ArrayPool<char>.Shared.Return(_array);
            }

            _array = a;
            _chars = a;
            return true;
        }
    }
}
