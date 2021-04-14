// © Microsoft Corporation. All rights reserved.

using System;

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
            return Fmt<T, object?, object?>(provider, arg, null, null, null, EstimateArgSize(arg));
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
            return Fmt<T0, T1, object?>(provider, arg0, arg1, null, null, EstimateArgSize(arg0) + EstimateArgSize(arg1));
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
            return Fmt<T0, T1, T2>(provider, arg0, arg1, arg2, null, EstimateArgSize(arg0) + EstimateArgSize(arg1) + EstimateArgSize(arg2));
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
            return Fmt<T0, T1, T2>(provider, arg0, arg1, arg2, args, EstimateArgSize(arg0) + EstimateArgSize(arg1) + EstimateArgSize(arg2) + EstimateArgSize(args));
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

            var estimatedSize = EstimateArgSize(args);

            return args!.Length switch
            {
                1 => Fmt<object?, object?, object?>(provider, args[0], null, null, null, estimatedSize),
                2 => Fmt<object?, object?, object?>(provider, args[0], args[1], null, null, estimatedSize),
                3 => Fmt<object?, object?, object?>(provider, args[0], args[1], args[2], null, estimatedSize),
                _ => Fmt<object?, object?, object?>(provider, args[0], args[1], args[2], args.AsSpan(3), estimatedSize),
            };
        }

        private string Fmt<T0, T1, T2>(IFormatProvider? provider, T0 arg0, T1 arg1, T2 arg2, ReadOnlySpan<object?> args, int estimatedSize)
        {
            estimatedSize += LiteralString.Length;
            var sm = (estimatedSize >= MaxStackAlloc) ? new StringMaker(estimatedSize) : new StringMaker(stackalloc char[MaxStackAlloc]);
            CoreFmt(ref sm, provider, arg0, arg1, arg2, args);
            return sm.ExtractString();
        }
    }
}
