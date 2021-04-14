// © Microsoft Corporation. All rights reserved.

using System;

namespace Text
{
    public readonly partial struct CompositeFormat
    {
        /// <summary>
        /// Formats a string with one argument.
        /// </summary>
        /// <typeparam name="T">Type of the single argument.</typeparam>
        /// <param name="destination">Where to write the resulting string.</param>
        /// <param name="charsWritten">The number of characters actually written to the destination span.</param>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="arg">An argument to use in the formatting operation.</param>
        /// <returns>True if there was enough room in teh destination span for the resulting string.</returns>
        public bool TryFormat<T>(Span<char> destination, out int charsWritten, IFormatProvider? provider, T arg)
        {
            CheckNumArgs(1, null);
            return TryFmt<T, object?, object?>(destination, out charsWritten, provider, arg, null, null, null);
        }

        /// <summary>
        /// Formats a string with two arguments.
        /// </summary>
        /// <typeparam name="T0">Type of the first argument.</typeparam>
        /// <typeparam name="T1">Type of the second argument.</typeparam>
        /// <param name="destination">Where to write the resulting string.</param>
        /// <param name="charsWritten">The number of characters actually written to the destination span.</param>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="arg0">First argument to use in the formatting operation.</param>
        /// <param name="arg1">Second argument to use in the formatting operation.</param>
        /// <returns>True if there was enough room in teh destination span for the resulting string.</returns>
        public bool TryFormat<T0, T1>(Span<char> destination, out int charsWritten, IFormatProvider? provider, T0 arg0, T1 arg1)
        {
            CheckNumArgs(2, null);
            return TryFmt<T0, T1, object?>(destination, out charsWritten, provider, arg0, arg1, null, null);
        }

        /// <summary>
        /// Formats a string with three arguments.
        /// </summary>
        /// <typeparam name="T0">Type of the first argument.</typeparam>
        /// <typeparam name="T1">Type of the second argument.</typeparam>
        /// <typeparam name="T2">Type of the third argument.</typeparam>
        /// <param name="destination">Where to write the resulting string.</param>
        /// <param name="charsWritten">The number of characters actually written to the destination span.</param>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="arg0">First argument to use in the formatting operation.</param>
        /// <param name="arg1">Second argument to use in the formatting operation.</param>
        /// <param name="arg2">Third argument to use in the formatting operation.</param>
        /// <returns>True if there was enough room in teh destination span for the resulting string.</returns>
        public bool TryFormat<T0, T1, T2>(Span<char> destination, out int charsWritten, IFormatProvider? provider, T0 arg0, T1 arg1, T2 arg2)
        {
            CheckNumArgs(3, null);
            return TryFmt<T0, T1, T2>(destination, out charsWritten, provider, arg0, arg1, arg2, null);
        }

        /// <summary>
        /// Formats a string with arguments.
        /// </summary>
        /// <typeparam name="T0">Type of the first argument.</typeparam>
        /// <typeparam name="T1">Type of the second argument.</typeparam>
        /// <typeparam name="T2">Type of the third argument.</typeparam>
        /// <param name="destination">Where to write the resulting string.</param>
        /// <param name="charsWritten">The number of characters actually written to the destination span.</param>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="arg0">First argument to use in the formatting operation.</param>
        /// <param name="arg1">Second argument to use in the formatting operation.</param>
        /// <param name="arg2">Third argument to use in the formatting operation.</param>
        /// <param name="args">Additional arguments to use in the formatting operation.</param>
        /// <returns>True if there was enough room in teh destination span for the resulting string.</returns>
        public bool TryFormat<T0, T1, T2>(Span<char> destination, out int charsWritten, IFormatProvider? provider, T0 arg0, T1 arg1, T2 arg2, params object?[]? args)
        {
            CheckNumArgs(3, args);
            return TryFmt<T0, T1, T2>(destination, out charsWritten, provider, arg0, arg1, arg2, args);
        }

        /// <summary>
        /// Formats a string with arguments.
        /// </summary>
        /// <param name="destination">Where to write the resulting string.</param>
        /// <param name="charsWritten">The number of characters actually written to the destination span.</param>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="args">Arguments to use in the formatting operation.</param>
        /// <returns>True if there was enough room in teh destination span for the resulting string.</returns>
        public bool TryFormat(Span<char> destination, out int charsWritten, IFormatProvider? provider, params object?[]? args)
        {
            CheckNumArgs(0, args);

            if (NumArgumentsNeeded == 0)
            {
                if (destination.Length < LiteralString.Length)
                {
                    charsWritten = 0;
                    return false;
                }

                LiteralString.AsSpan().CopyTo(destination);
                charsWritten = LiteralString.Length;
                return true;
            }

            return args!.Length switch
            {
                1 => TryFmt<object?, object?, object?>(destination, out charsWritten, provider, args[0], null, null, null),
                2 => TryFmt<object?, object?, object?>(destination, out charsWritten, provider, args[0], args[1], null, null),
                3 => TryFmt<object?, object?, object?>(destination, out charsWritten, provider, args[0], args[1], args[2], null),
                _ => TryFmt<object?, object?, object?>(destination, out charsWritten, provider, args[0], args[1], args[2], args.AsSpan(3)),
            };
        }

        private bool TryFmt<T0, T1, T2>(Span<char> destination, out int charsWritten, IFormatProvider? provider, T0 arg0, T1 arg1, T2 arg2, ReadOnlySpan<object?> args)
        {
            var sm = new StringMaker(destination, true);
            CoreFmt(ref sm, provider, arg0, arg1, arg2, args);
            charsWritten = sm.Length;
            var overflowed = sm.Overflowed;
            return !overflowed;
        }
    }
}
