// © Microsoft Corporation. All rights reserved.

namespace System.Text
{
    /// <summary>
    /// A chunk of formatting information.
    /// </summary>
    internal readonly struct FormatterSegment
    {
        public FormatterSegment(string format, short width, short literalCount, short argIndex)
        {
            Format = format;
            Width = width;
            LiteralCount = literalCount;
            ArgIndex = argIndex;
        }

        /// <summary>
        /// The number of bytes of literal text consumed by this segment.
        /// </summary>
        public short LiteralCount { get; }

        /// <summary>
        /// The index of the argument to be formatted, -1 to skip argument formatting.
        /// </summary>
        public short ArgIndex { get; }

        /// <summary>
        /// The width of the formatted value in characters. If this is negative, it indicates to right-justify
        /// and the field width is then the absolute value.
        /// </summary>
        public short Width { get; }

        /// <summary>
        /// The custom format string to format the argument with.
        /// </summary>
        public string Format { get; }
    }
}
