// © Microsoft Corporation. All rights reserved.

namespace Text
{
    public readonly partial struct StringFormatter
    {
        /// <summary>
        /// A chunk of formatting information.
        /// </summary>
        private readonly struct Segment
        {
            public Segment(short literalCount, short argIndex, short argWidth, string argFormat)
            {
                LiteralCount = literalCount;
                ArgIndex = argIndex;
                ArgWidth = argWidth;
                ArgFormat = argFormat;
            }

            /// <summary>
            /// Gets the number of chars of literal text consumed by this segment.
            /// </summary>
            public short LiteralCount { get; }

            /// <summary>
            /// Gets the index of the argument to be formatted, -1 to skip argument formatting.
            /// </summary>
            public short ArgIndex { get; }

            /// <summary>
            /// Gets the width of the formatted value in characters. If this is negative, it indicates to left-justify
            /// and the field width is then the absolute value.
            /// </summary>
            public short ArgWidth { get; }

            /// <summary>
            /// Gets the custom format string to use when formatting the argument.
            /// </summary>
            public string ArgFormat { get; }
        }
    }
}
