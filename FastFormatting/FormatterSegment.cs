// © Microsoft Corporation. All rights reserved.

namespace FastFormatting
{
    /// <summary>
    /// A chunk of formatting information.
    /// </summary>
    internal readonly struct FormatterSegment
    {
        public FormatterSegment(short literalCount, short argIndex, short argWidth, string argFormat)
        {
            LiteralCount = literalCount;
            ArgIndex = argIndex;
            ArgWidth = argWidth;
            ArgFormat = argFormat;
        }

        /// <summary>
        /// The number of chars of literal text consumed by this segment.
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
        public short ArgWidth { get; }

        /// <summary>
        /// The custom format string to use when formatting the argument.
        /// </summary>
        public string ArgFormat { get; }
    }
}
