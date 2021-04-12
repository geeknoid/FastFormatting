// © Microsoft Corporation. All rights reserved.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Text
{
    public readonly partial struct CompositeFormat
    {
        /// <summary>
        /// Formats a string with a single argument.
        /// </summary>
        /// <typeparam name="T">Type of the single argument.</typeparam>
        /// <param name="arg">An argument to use in the formatting operation.</param>
        /// <returns>The formatted string.</returns>
        public string Format<T>(T arg) => Format<T>(null, arg);

        /// <summary>
        /// Formats a string with a single argument.
        /// </summary>
        /// <typeparam name="T">Type of the single argument.</typeparam>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="arg">An argument to use in the formatting operation.</param>
        /// <returns>The formatted string.</returns>
        public string Format<T>(IFormatProvider? provider, T arg)
        {
            CheckNumArgs(1, null);
            var estimatedSize = LiteralString.Length + EstimateArgSize(arg);
            var sm = (estimatedSize >= MaxStackAlloc) ? new StringMaker(estimatedSize) : new StringMaker(stackalloc char[MaxStackAlloc]);
            Format<T, object?, object?>(ref sm, provider, arg, null, null, Array.Empty<object>());
            return sm.ExtractString();
        }

        /// <summary>
        /// Formats a string with two arguments.
        /// </summary>
        /// <typeparam name="T0">Type of the first argument.</typeparam>
        /// <typeparam name="T1">Type of the second argument.</typeparam>
        /// <param name="arg0">First argument to use in the formatting operation.</param>
        /// <param name="arg1">Second argument to use in the formatting operation.</param>
        /// <returns>The formatted string.</returns>
        public string Format<T0, T1>(T0 arg0, T1 arg1) => Format<T0, T1>(null, arg0, arg1);

        /// <summary>
        /// Formats a string with two arguments.
        /// </summary>
        /// <typeparam name="T0">Type of the first argument.</typeparam>
        /// <typeparam name="T1">Type of the second argument.</typeparam>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="arg0">First argument to use in the formatting operation.</param>
        /// <param name="arg1">Second argument to use in the formatting operation.</param>
        /// <returns>The formatted string.</returns>
        public string Format<T0, T1>(IFormatProvider? provider, T0 arg0, T1 arg1)
        {
            CheckNumArgs(2, null);
            var estimatedSize = LiteralString.Length + EstimateArgSize(arg0) + EstimateArgSize(arg1);
            var sm = (estimatedSize >= MaxStackAlloc) ? new StringMaker(estimatedSize) : new StringMaker(stackalloc char[MaxStackAlloc]);
            Format<T0, T1, object?>(ref sm, provider, arg0, arg1, null, Array.Empty<object>());
            return sm.ExtractString();
        }

        /// <summary>
        /// Formats a string with three arguments.
        /// </summary>
        /// <typeparam name="T0">Type of the first argument.</typeparam>
        /// <typeparam name="T1">Type of the second argument.</typeparam>
        /// <typeparam name="T2">Type of the third argument.</typeparam>
        /// <param name="arg0">First argument to use in the formatting operation.</param>
        /// <param name="arg1">Second argument to use in the formatting operation.</param>
        /// <param name="arg2">Third argument to use in the formatting operation.</param>
        /// <returns>The formatted string.</returns>
        public string Format<T0, T1, T2>(T0 arg0, T1 arg1, T2 arg2) => Format<T0, T1, T2>(null, arg0, arg1, arg2);

        /// <summary>
        /// Formats a string with three arguments.
        /// </summary>
        /// <typeparam name="T0">Type of the first argument.</typeparam>
        /// <typeparam name="T1">Type of the second argument.</typeparam>
        /// <typeparam name="T2">Type of the third argument.</typeparam>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="arg0">First argument to use in the formatting operation.</param>
        /// <param name="arg1">Second argument to use in the formatting operation.</param>
        /// <param name="arg2">Third argument to use in the formatting operation.</param>
        /// <returns>The formatted string.</returns>
        public string Format<T0, T1, T2>(IFormatProvider? provider, T0 arg0, T1 arg1, T2 arg2)
        {
            CheckNumArgs(3, null);
            var estimatedSize = LiteralString.Length + EstimateArgSize(arg0) + EstimateArgSize(arg1) + EstimateArgSize(arg2);
            var sm = (estimatedSize >= MaxStackAlloc) ? new StringMaker(estimatedSize) : new StringMaker(stackalloc char[MaxStackAlloc]);
            Format<T0, T1, T2>(ref sm, provider, arg0, arg1, arg2, Array.Empty<object>());
            return sm.ExtractString();
        }

        /// <summary>
        /// Formats a string with arguments.
        /// </summary>
        /// <typeparam name="T0">Type of the first argument.</typeparam>
        /// <typeparam name="T1">Type of the second argument.</typeparam>
        /// <typeparam name="T2">Type of the third argument.</typeparam>
        /// <param name="arg0">First argument to use in the formatting operation.</param>
        /// <param name="arg1">Second argument to use in the formatting operation.</param>
        /// <param name="arg2">Third argument to use in the formatting operation.</param>
        /// <param name="args">Additional arguments to use in the formatting operation.</param>
        /// <returns>The formatted string.</returns>
        public string Format<T0, T1, T2>(T0 arg0, T1 arg1, T2 arg2, params object?[]? args) => Format<T0, T1, T2>(null, arg0, arg1, arg2, args);

        /// <summary>
        /// Formats a string with arguments.
        /// </summary>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <typeparam name="T0">Type of the first argument.</typeparam>
        /// <typeparam name="T1">Type of the second argument.</typeparam>
        /// <typeparam name="T2">Type of the third argument.</typeparam>
        /// <param name="arg0">First argument to use in the formatting operation.</param>
        /// <param name="arg1">Second argument to use in the formatting operation.</param>
        /// <param name="arg2">Third argument to use in the formatting operation.</param>
        /// <param name="args">Additional arguments to use in the formatting operation.</param>
        /// <returns>The formatted string.</returns>
        public string Format<T0, T1, T2>(IFormatProvider? provider, T0 arg0, T1 arg1, T2 arg2, params object?[]? args)
        {
            CheckNumArgs(3, args);
            var estimatedSize = LiteralString.Length + EstimateArgSize(arg0) + EstimateArgSize(arg1) + EstimateArgSize(arg2) + EstimateArgSize(args);
            var sm = (estimatedSize >= MaxStackAlloc) ? new StringMaker(estimatedSize) : new StringMaker(stackalloc char[MaxStackAlloc]);
            Format<T0, T1, T2>(ref sm, provider, arg0, arg1, arg2, args);
            return sm.ExtractString();
        }

        /// <summary>
        /// Formats a string with arguments.
        /// </summary>
        /// <param name="args">Arguments to use in the formatting operation.</param>
        /// <returns>The formatted string.</returns>
        public string Format(params object?[]? args) => Format(null, args);

        /// <summary>
        /// Formats a string with arguments.
        /// </summary>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="args">Arguments to use in the formatting operation.</param>
        /// <returns>The formatted string.</returns>
        public string Format(IFormatProvider? provider, params object?[]? args)
        {
            CheckNumArgs(0, args);

            if (NumArgumentsNeeded == 0)
            {
                return LiteralString;
            }

            var estimatedSize = LiteralString.Length + EstimateArgSize(args);
            var sm = (estimatedSize >= MaxStackAlloc) ? new StringMaker(estimatedSize) : new StringMaker(stackalloc char[MaxStackAlloc]);

            switch (args!.Length)
            {
                case 1:
                    Format<object?, object?, object?>(ref sm, provider, args[0], null, null, Array.Empty<object>());
                    break;

                case 2:
                    Format<object?, object?, object?>(ref sm, provider, args[0], args[1], null, Array.Empty<object>());
                    break;

                case 3:
                    Format<object?, object?, object?>(ref sm, provider, args[0], args[1], args[2], Array.Empty<object>());
                    break;

                default:
                    Format<object?, object?, object?>(ref sm, provider, args[0], args[1], args[2], args.AsSpan(3));
                    break;
            }

            return sm.ExtractString();
        }

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
            var sm = new StringMaker(destination, true);
            Format<T, object?, object?>(ref sm, provider, arg, null, null, Array.Empty<object>());
            charsWritten = sm.Length;
            var overflowed = sm.Overflowed;
            sm.Dispose();
            return !overflowed;
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
            var sm = new StringMaker(destination, true);
            Format<T0, T1, object?>(ref sm, provider, arg0, arg1, null, Array.Empty<object>());
            charsWritten = sm.Length;
            var overflowed = sm.Overflowed;
            sm.Dispose();
            return !overflowed;
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
            var sm = new StringMaker(destination, true);
            Format<T0, T1, T2>(ref sm, provider, arg0, arg1, arg2, Array.Empty<object>());
            charsWritten = sm.Length;
            var overflowed = sm.Overflowed;
            sm.Dispose();
            return !overflowed;
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
            var sm = new StringMaker(destination, true);
            Format<T0, T1, T2>(ref sm, provider, arg0, arg1, arg2, args);
            charsWritten = sm.Length;
            var overflowed = sm.Overflowed;
            sm.Dispose();
            return !overflowed;
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

            var sm = new StringMaker(destination, true);
            switch (args!.Length)
            {
                case 1:
                    Format<object?, object?, object?>(ref sm, provider, args[0], null, null, Array.Empty<object>());
                    break;

                case 2:
                    Format<object?, object?, object?>(ref sm, provider, args[0], args[1], null, Array.Empty<object>());
                    break;

                case 3:
                    Format<object?, object?, object?>(ref sm, provider, args[0], args[1], args[2], Array.Empty<object>());
                    break;

                default:
                    Format<object?, object?, object?>(ref sm, provider, args[0], args[1], args[2], args.AsSpan(3));
                    break;
            }

            charsWritten = sm.Length;
            var overflowed = sm.Overflowed;
            sm.Dispose();
            return !overflowed;
        }
    }
}
