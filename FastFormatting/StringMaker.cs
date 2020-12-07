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
        private bool _allowExpansion;
        private bool _overflowed;

        public StringMaker(Span<char> initialBuffer, bool allowExpansion = true)
        {
            _array = null;
            _chars = initialBuffer;
            _length = 0;
            _allowExpansion = allowExpansion;
            _overflowed = false;
        }

        public StringMaker(int initialCapacity = DefaultCapacity)
        {
            if (initialCapacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(initialCapacity));
            }

            _array = ArrayPool<char>.Shared.Rent(initialCapacity);
            _chars = _array;
            _length = 0;
            _allowExpansion = true;
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

        public StringMaker TestOverflow()
        {
            if (_overflowed)
            {
                throw new OverflowException();  // BUGBUG: not the right exception, but a good placeholder for now
            }

            return this;
        }

        public int Length => _length;
        public bool Overflowed => _overflowed;

        public StringMaker Fill(char value, int count)
        {
            if (_length > _chars.Length - count)
            {
                if (!Expand(count)) return this;
            }

            _chars.Slice(_length, count).Fill(value);
            _length += count;
            return this;
        }

        public StringMaker Append(string? value) => Append(value, 0);

        public StringMaker Append(string? value, int width)
        {
            if (value == null)
            {
                return FinishAppend(string.Empty, width);
            }

            if (width > value.Length)
            {
                // optimize the right-aligned case by avoiding a copy
                Fill(' ', width - value.Length);
                return FinishAppend(value, 0);
            }

            return FinishAppend(value, width);
        }

        public StringMaker Append(ReadOnlySpan<char> value) => Append(value, 0);

        public StringMaker Append(ReadOnlySpan<char> value, int width)
        {
            if (_length > _chars.Length - value.Length)
            {
                if (!Expand(value.Length)) return this;
            }

            if (width > value.Length)
            {
                // optimize the right-aligned case by avoiding a copy
                Fill(' ', width - value.Length);
                return FinishAppend(value, 0);
            }

            return FinishAppend(value, width);
        }

        public StringMaker Append(char value)
        {
            if (_length == _chars.Length)
            {
                if (!Expand(1)) return this;
            }

            _chars[_length++] = value;
            return this;
        }

        public StringMaker Append(char value, int width)
        {
            if (_length == _chars.Length)
            {
                if (!Expand(1)) return this;
            }

            if (width > 1)
            {
                // optimize the right-aligned case by avoiding a copy
                Fill(' ', width - 1);
                _chars[_length] = value;
                return FinishAppend(1, 0);
            }

            _chars[_length] = value;
            return FinishAppend(1, width);
        }

        public StringMaker Append(sbyte value) => Append(value, string.Empty, null, 0);
        public StringMaker Append(short value) => Append(value, string.Empty, null, 0);
        public StringMaker Append(int value) => Append(value, string.Empty, null, 0);
        public StringMaker Append(long value) => Append(value, string.Empty, null, 0);

        public StringMaker Append(long value, string format, IFormatProvider? provider, int width)
        {
            int charsWritten;
            while (!value.TryFormat(_chars.Slice(_length), out charsWritten, format, provider))
            {
                if (!Expand()) return this;
            }

            return FinishAppend(charsWritten, width);
        }

        public StringMaker Append(byte value) => Append(value, string.Empty, null, 0);
        public StringMaker Append(ushort value) => Append(value, string.Empty, null, 0);
        public StringMaker Append(uint value) => Append(value, string.Empty, null, 0);
        public StringMaker Append(ulong value) => Append(value, string.Empty, null, 0);

        public StringMaker Append(ulong value, string format, IFormatProvider? provider, int width)
        {
            int charsWritten;
            while (!value.TryFormat(_chars.Slice(_length), out charsWritten, format, provider))
            {
                if (!Expand()) return this;
            }

            return FinishAppend(charsWritten, width);
        }

        public StringMaker Append(float value) => Append(value, string.Empty, null, 0);
        public StringMaker Append(double value) => Append(value, string.Empty, null, 0);

        public StringMaker Append(double value, string format, IFormatProvider? provider, int width)
        {
            int charsWritten;
            while (!value.TryFormat(_chars.Slice(_length), out charsWritten, format, provider))
            {
                if (!Expand()) return this;
            }

            return FinishAppend(charsWritten, width);
        }

        public StringMaker Append(bool value) => Append(value, 0);

        public StringMaker Append(bool value, int width)
        {
            int charsWritten;
            while (!value.TryFormat(_chars.Slice(_length), out charsWritten))
            {
                if (!Expand()) return this;
            }

            return FinishAppend(charsWritten, width);
        }

        public StringMaker Append(decimal value) => Append(value, string.Empty, null, 0);

        public StringMaker Append(decimal value, string format, IFormatProvider? provider, int width)
        {
            int charsWritten;
            while (!value.TryFormat(_chars.Slice(_length), out charsWritten, format, provider))
            {
                if (!Expand()) return this;
            }

            return FinishAppend(charsWritten, width);
        }

        public StringMaker Append(DateTime value) => Append(value, string.Empty, null, 0);

        public StringMaker Append(DateTime value, string format, IFormatProvider? provider, int width)
        {
            int charsWritten;
            while (!value.TryFormat(_chars.Slice(_length), out charsWritten, format, provider))
            {
                if (!Expand()) return this;
            }

            return FinishAppend(charsWritten, width);
        }

        public StringMaker Append(TimeSpan value) => Append(value, string.Empty, null, 0);

        public StringMaker Append(TimeSpan value, string format, IFormatProvider? provider, int width)
        {
            int charsWritten;
            while (!value.TryFormat(_chars.Slice(_length), out charsWritten, format, provider))
            {
                if (!Expand()) return this;
            }

            return FinishAppend(charsWritten, width);
        }

        public StringMaker Append<T>(T value, string format, IFormatProvider? provider, int width) where T : ISpanFormattable
        {
            int charsWritten;
            while (!value.TryFormat(_chars.Slice(_length), out charsWritten, format, provider))
            {
                if (!Expand()) return this;
            }

            return FinishAppend(charsWritten, width);
        }

        public StringMaker Append(IFormattable value, string format, IFormatProvider? provider, int width)
        {
            return FinishAppend(value.ToString(format, provider), width);
        }

        public StringMaker Append(object? value, int width)
        {
            if (value == null)
            {
                return this;
            }
            return FinishAppend(value.ToString(), width);
        }

        private StringMaker FinishAppend(int charsWritten, int width)
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
                    if (!Expand(padding)) return this;
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

            return this;
        }

        private StringMaker FinishAppend(ReadOnlySpan<char> result, int width)
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
                if (!Expand(extra)) return this;
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
            return this;
        }

        private bool Expand(int neededCapacity = 0)
        {
            if (!_allowExpansion)
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
