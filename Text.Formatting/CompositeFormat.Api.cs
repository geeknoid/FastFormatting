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
            var pa = new Params<T, Nothing, Nothing>(arg, default, default);
            return Format(provider, in pa);
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
            var pa = new Params<T0, T1, Nothing>(arg0, arg1, default);
            return Format(provider, in pa);
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
            var pa = new Params<T0, T1, T2>(arg0, arg1, arg2);
            return Format(provider, in pa);
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
            var pa = new Params<T0, T1, T2>(arg0, arg1, arg2, args);
            return Format(provider, in pa);
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

            var pa = args!.Length switch
            {
                1 => new Params<object?, object?, object?>(args[0], null, null),
                2 => new Params<object?, object?, object?>(args[0], args[1], null),
                3 => new Params<object?, object?, object?>(args[0], args[1], args[2]),
                _ => new Params<object?, object?, object?>(args[0], args[1], args[2], args.AsSpan(3))
            };

            return Format(provider, in pa);
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
            var pa = new Params<T, Nothing, Nothing>(arg, default, default);
            return TryFormat(destination, out charsWritten, provider, in pa);
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
            var pa = new Params<T0, T1, Nothing>(arg0, arg1, default);
            return TryFormat(destination, out charsWritten, provider, in pa);
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
            var pa = new Params<T0, T1, T2>(arg0, arg1, arg2);
            return TryFormat(destination, out charsWritten, provider, in pa);
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
            var pa = new Params<T0, T1, T2>(arg0, arg1, arg2, args);
            return TryFormat(destination, out charsWritten, provider, in pa);
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

            var pa = args!.Length switch
            {
                1 => new Params<object?, object?, object?>(args[0], null, null),
                2 => new Params<object?, object?, object?>(args[0], args[1], null),
                3 => new Params<object?, object?, object?>(args[0], args[1], args[2]),
                _ => new Params<object?, object?, object?>(args[0], args[1], args[2], args.AsSpan(3))
            };

            return TryFormat(destination, out charsWritten, provider, in pa);
        }

        private string Format<T0, T1, T2>(IFormatProvider? provider, in Params<T0, T1, T2> pa)
        {
            var estimatedSize = EstimateResultSize(in pa);
            var sm = (estimatedSize >= MaxStackAlloc) ? new StringMaker(estimatedSize) : new StringMaker(stackalloc char[MaxStackAlloc]);
            Format<T0, T1, T2>(ref sm, provider, in pa);
            var result = sm.ExtractString();
            sm.Dispose();
            return result;
        }

        private bool TryFormat<T0, T1, T2>(Span<char> destination, out int charsWritten, IFormatProvider? provider, in Params<T0, T1, T2> pa)
        {
            var sm = new StringMaker(destination, true);
            Format<T0, T1, T2>(ref sm, provider, in pa);
            charsWritten = sm.Length;
            var overflowed = sm.Overflowed;
            sm.Dispose();
            return !overflowed;
        }
    }
}