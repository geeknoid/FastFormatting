// © Microsoft Corporation. All rights reserved.

namespace System.Text
{
    /// <summary>
    /// A chunk of formatting information.
    /// </summary>
    internal readonly struct FormatterSegment
    {
        FormatterSegment(string? format, short width, bool leftJustify, byte literalCount, byte argIndex)
        {
            Format = format;
            Width = width;
            LeftJustify = leftJustify;
            LiteralCount = literalCount;
            ArgIndex = argIndex;
        }

        public static FormatterSegment Literal(byte literalCount)
        {
            return new FormatterSegment(null, 0, false, literalCount, 0);
        }

        public static FormatterSegment Full(string? format, short width, bool leftJustify, byte argIndex)
        {
            return new FormatterSegment(format, width, leftJustify, 0, argIndex);
        }

        /// <summary>
        /// The number of bytes of literal text consumed by this segment.
        /// </summary>
        public int LiteralCount { get; }

        /// <summary>
        /// The index of the argument to be formatted.
        /// </summary>
        public int ArgIndex { get; }

        /// <summary>
        /// The width of the formatted value in characters. If this is negative, it indicates instead the
        /// argument that supplies the width.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Whether the formatted value should be left justified within the field or not. Note that
        /// when the Width is negative, the resulting field width from reading the argument determines whether
        /// to left justify or not, and this value is ignored.
        /// </summary>
        public bool LeftJustify { get; }

        /// <summary>
        /// The custom format string to format the argument with.
        /// </summary>
        public string? Format { get; }
    }
}
