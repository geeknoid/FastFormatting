using System;

namespace FastFormatting
{
    public interface ISpanFormattable
    {
        bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider);
    }
}
