// © Microsoft Corporation. All rights reserved.

using System;
using System.Text;

namespace Text
{
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// Formats a string with a single argument.
        /// </summary>
        /// <typeparam name="T">Type of the single argument.</typeparam>
        /// <param name="arg">An argument to use in the formatting operation.</param>
        /// <returns>The formatted string.</returns>
        public static StringBuilder AppendFormat<T>(this StringBuilder sb, CompositeFormat format, T arg) => AppendFormat<T>(sb, format, null, arg);

        /// <summary>
        /// Formats a string with a single argument.
        /// </summary>
        /// <typeparam name="T">Type of the single argument.</typeparam>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="arg">An argument to use in the formatting operation.</param>
        /// <returns>The formatted string.</returns>
        public static StringBuilder AppendFormat<T>(this StringBuilder sb, CompositeFormat format, IFormatProvider? provider, T arg)
        {
            format.CheckNumArgs(1, null);
            var pa = new Params<T, Nothing, Nothing>(arg, default, default);
            return AppendFormat(sb, format, provider, in pa);
        }

        /// <summary>
        /// Formats a string with two arguments.
        /// </summary>
        /// <typeparam name="T0">Type of the first argument.</typeparam>
        /// <typeparam name="T1">Type of the second argument.</typeparam>
        /// <param name="arg0">First argument to use in the formatting operation.</param>
        /// <param name="arg1">Second argument to use in the formatting operation.</param>
        /// <returns>The formatted string.</returns>
        public static StringBuilder AppendFormat<T0, T1>(this StringBuilder sb, CompositeFormat format, T0 arg0, T1 arg1) => AppendFormat<T0, T1>(sb, format, null, arg0, arg1);

        /// <summary>
        /// Formats a string with two arguments.
        /// </summary>
        /// <typeparam name="T0">Type of the first argument.</typeparam>
        /// <typeparam name="T1">Type of the second argument.</typeparam>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="arg0">First argument to use in the formatting operation.</param>
        /// <param name="arg1">Second argument to use in the formatting operation.</param>
        /// <returns>The formatted string.</returns>
        public static StringBuilder AppendFormat<T0, T1>(this StringBuilder sb, CompositeFormat format, IFormatProvider? provider, T0 arg0, T1 arg1)
        {
            format.CheckNumArgs(2, null);
            var pa = new Params<T0, T1, Nothing>(arg0, arg1, default);
            return AppendFormat(sb, format, provider, in pa);
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
        public static StringBuilder AppendFormat<T0, T1, T2>(this StringBuilder sb, CompositeFormat format, T0 arg0, T1 arg1, T2 arg2) => AppendFormat<T0, T1, T2>(sb, format, null, arg0, arg1, arg2);

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
        public static StringBuilder AppendFormat<T0, T1, T2>(this StringBuilder sb, CompositeFormat format,IFormatProvider? provider, T0 arg0, T1 arg1, T2 arg2)
        {
            format.CheckNumArgs(3, null);
            var pa = new Params<T0, T1, T2>(arg0, arg1, arg2);
            return AppendFormat(sb, format, provider, in pa);
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
        public static StringBuilder AppendFormat<T0, T1, T2>(this StringBuilder sb, CompositeFormat format, T0 arg0, T1 arg1, T2 arg2, params object?[]? args) => AppendFormat<T0, T1, T2>(sb, format, null, arg0, arg1, arg2, args);

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
        public static StringBuilder AppendFormat<T0, T1, T2>(this StringBuilder sb, CompositeFormat format, IFormatProvider? provider, T0 arg0, T1 arg1, T2 arg2, params object?[]? args)
        {
            format.CheckNumArgs(3, args);
            var pa = new Params<T0, T1, T2>(arg0, arg1, arg2, args);
            return AppendFormat(sb, format, provider, in pa);
        }

        /// <summary>
        /// Formats a string with arguments.
        /// </summary>
        /// <param name="args">Arguments to use in the formatting operation.</param>
        /// <returns>The formatted string.</returns>
        public static StringBuilder AppendFormat(this StringBuilder sb, CompositeFormat format, params object?[]? args) => AppendFormat(sb, format, null, args);

        /// <summary>
        /// Formats a string with arguments.
        /// </summary>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="args">Arguments to use in the formatting operation.</param>
        /// <returns>The formatted string.</returns>
        public static StringBuilder AppendFormat(this StringBuilder sb, CompositeFormat format, IFormatProvider? provider, params object?[]? args)
        {
            format.CheckNumArgs(0, args);

            if (format.NumArgumentsNeeded == 0)
            {
                return sb.Append(format.LiteralString);
            }

            var pa = args!.Length switch
            {
                1 => new Params<object?, object?, object?>(args[0], null, null),
                2 => new Params<object?, object?, object?>(args[0], args[1], null),
                3 => new Params<object?, object?, object?>(args[0], args[1], args[2]),
                _ => new Params<object?, object?, object?>(args[0], args[1], args[2], args.AsSpan(3))
            };

            return AppendFormat(sb, format, provider, in pa);
        }

        private static StringBuilder AppendFormat<T0, T1, T2>(StringBuilder sb, CompositeFormat format, IFormatProvider? provider, in Params<T0, T1, T2> pa)
        {
            var estimatedSize = format.EstimateResultSize(in pa);
            var sm = (estimatedSize >= CompositeFormat.MaxStackAlloc) ? new StringMaker(estimatedSize) : new StringMaker(stackalloc char[CompositeFormat.MaxStackAlloc]);
            format.Format<T0, T1, T2>(ref sm, provider, in pa);
            sm.AppendTo(sb);
            sm.Dispose();

            return sb;
        }
    }
}
