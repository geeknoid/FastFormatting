// © Microsoft Corporation. All rights reserved.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Text
{
    /// <summary>
    /// Extensions for accelerated formatting on <see cref="StringBuilder" />.
    /// </summary>
    [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Handled downstream")]
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// Formats a string with a single argument.
        /// </summary>
        /// <typeparam name="T">Type of the single argument.</typeparam>
        /// <param name="sb">The string builder to append to.</param>
        /// <param name="format">The composite format to apply.</param>
        /// <param name="arg">An argument to use in the formatting operation.</param>
        /// <returns>The input string builder for call chaining.</returns>
        public static StringBuilder AppendFormat<T>(this StringBuilder sb, CompositeFormat format, T arg)
            => format.AppendFormat<T>(sb, null, arg);

        /// <summary>
        /// Formats a string with a single argument.
        /// </summary>
        /// <typeparam name="T">Type of the single argument.</typeparam>
        /// <param name="sb">The string builder to append to.</param>
        /// <param name="format">The composite format to apply.</param>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="arg">An argument to use in the formatting operation.</param>
        /// <returns>The input string builder for call chaining.</returns>
        public static StringBuilder AppendFormat<T>(this StringBuilder sb, CompositeFormat format, IFormatProvider? provider, T arg)
            => format.AppendFormat<T>(sb, provider, arg);

        /// <summary>
        /// Formats a string with two arguments.
        /// </summary>
        /// <typeparam name="T0">Type of the first argument.</typeparam>
        /// <typeparam name="T1">Type of the second argument.</typeparam>
        /// <param name="sb">The string builder to append to.</param>
        /// <param name="format">The composite format to apply.</param>
        /// <param name="arg0">First argument to use in the formatting operation.</param>
        /// <param name="arg1">Second argument to use in the formatting operation.</param>
        /// <returns>The input string builder for call chaining.</returns>
        public static StringBuilder AppendFormat<T0, T1>(this StringBuilder sb, CompositeFormat format, T0 arg0, T1 arg1)
            => format.AppendFormat<T0, T1>(sb, null, arg0, arg1);

        /// <summary>
        /// Formats a string with two arguments.
        /// </summary>
        /// <typeparam name="T0">Type of the first argument.</typeparam>
        /// <typeparam name="T1">Type of the second argument.</typeparam>
        /// <param name="sb">The string builder to append to.</param>
        /// <param name="format">The composite format to apply.</param>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="arg0">First argument to use in the formatting operation.</param>
        /// <param name="arg1">Second argument to use in the formatting operation.</param>
        /// <returns>The input string builder for call chaining.</returns>
        public static StringBuilder AppendFormat<T0, T1>(this StringBuilder sb, CompositeFormat format, IFormatProvider? provider, T0 arg0, T1 arg1)
            => format.AppendFormat<T0, T1>(sb, provider, arg0, arg1);

        /// <summary>
        /// Formats a string with three arguments.
        /// </summary>
        /// <typeparam name="T0">Type of the first argument.</typeparam>
        /// <typeparam name="T1">Type of the second argument.</typeparam>
        /// <typeparam name="T2">Type of the third argument.</typeparam>
        /// <param name="sb">The string builder to append to.</param>
        /// <param name="format">The composite format to apply.</param>
        /// <param name="arg0">First argument to use in the formatting operation.</param>
        /// <param name="arg1">Second argument to use in the formatting operation.</param>
        /// <param name="arg2">Third argument to use in the formatting operation.</param>
        /// <returns>The input string builder for call chaining.</returns>
        public static StringBuilder AppendFormat<T0, T1, T2>(this StringBuilder sb, CompositeFormat format, T0 arg0, T1 arg1, T2 arg2)
            => format.AppendFormat<T0, T1, T2>(sb, null, arg0, arg1, arg2);

        /// <summary>
        /// Formats a string with three arguments.
        /// </summary>
        /// <typeparam name="T0">Type of the first argument.</typeparam>
        /// <typeparam name="T1">Type of the second argument.</typeparam>
        /// <typeparam name="T2">Type of the third argument.</typeparam>
        /// <param name="sb">The string builder to append to.</param>
        /// <param name="format">The composite format to apply.</param>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="arg0">First argument to use in the formatting operation.</param>
        /// <param name="arg1">Second argument to use in the formatting operation.</param>
        /// <param name="arg2">Third argument to use in the formatting operation.</param>
        /// <returns>The input string builder for call chaining.</returns>
        public static StringBuilder AppendFormat<T0, T1, T2>(this StringBuilder sb, CompositeFormat format, IFormatProvider? provider, T0 arg0, T1 arg1, T2 arg2)
            => format.AppendFormat<T0, T1, T2>(sb, provider, arg0, arg1, arg2);

        /// <summary>
        /// Formats a string with arguments.
        /// </summary>
        /// <typeparam name="T0">Type of the first argument.</typeparam>
        /// <typeparam name="T1">Type of the second argument.</typeparam>
        /// <typeparam name="T2">Type of the third argument.</typeparam>
        /// <param name="sb">The string builder to append to.</param>
        /// <param name="format">The composite format to apply.</param>
        /// <param name="arg0">First argument to use in the formatting operation.</param>
        /// <param name="arg1">Second argument to use in the formatting operation.</param>
        /// <param name="arg2">Third argument to use in the formatting operation.</param>
        /// <param name="args">Additional arguments to use in the formatting operation.</param>
        /// <returns>The input string builder for call chaining.</returns>
        public static StringBuilder AppendFormat<T0, T1, T2>(this StringBuilder sb, CompositeFormat format, T0 arg0, T1 arg1, T2 arg2, params object?[]? args)
            => format.AppendFormat<T0, T1, T2>(sb, null, arg0, arg1, arg2, args);

        /// <summary>
        /// Formats a string with arguments.
        /// </summary>
        /// <typeparam name="T0">Type of the first argument.</typeparam>
        /// <typeparam name="T1">Type of the second argument.</typeparam>
        /// <typeparam name="T2">Type of the third argument.</typeparam>
        /// <param name="sb">The string builder to append to.</param>
        /// <param name="format">The composite format to apply.</param>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="arg0">First argument to use in the formatting operation.</param>
        /// <param name="arg1">Second argument to use in the formatting operation.</param>
        /// <param name="arg2">Third argument to use in the formatting operation.</param>
        /// <param name="args">Additional arguments to use in the formatting operation.</param>
        /// <returns>The input string builder for call chaining.</returns>
        public static StringBuilder AppendFormat<T0, T1, T2>(this StringBuilder sb, CompositeFormat format, IFormatProvider? provider, T0 arg0, T1 arg1, T2 arg2, params object?[]? args)
            => format.AppendFormat<T0, T1, T2>(sb, provider, arg0, arg1, arg2, args);

        /// <summary>
        /// Formats a string with arguments.
        /// </summary>
        /// <param name="sb">The string builder to append to.</param>
        /// <param name="format">The composite format to apply.</param>
        /// <param name="args">Arguments to use in the formatting operation.</param>
        /// <returns>The input string builder for call chaining.</returns>
        public static StringBuilder AppendFormat(this StringBuilder sb, CompositeFormat format, params object?[]? args)
            => format.AppendFormat(sb, null, args);

        /// <summary>
        /// Formats a string with arguments.
        /// </summary>
        /// <param name="sb">The string builder to append to.</param>
        /// <param name="format">The composite format to apply.</param>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="args">Arguments to use in the formatting operation.</param>
        /// <returns>The input string builder for call chaining.</returns>
        public static StringBuilder AppendFormat(this StringBuilder sb, CompositeFormat format, IFormatProvider? provider, params object?[]? args)
            => format.AppendFormat(sb, provider, args);
    }
}
