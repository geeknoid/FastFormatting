// © Microsoft Corporation. All rights reserved.

#if STATIC_FORMAT
using System;
using System.Collections.Concurrent;

#pragma warning disable S3872 // Parameter names should not duplicate the names of their methods
#pragma warning disable SA1011 // Closing square brackets should be spaced correctly

namespace Text
{
    public readonly partial struct StringFormatter
    {
        private const int MaxCacheEntries = 128;

        private static readonly ConcurrentDictionary<string, StringFormatter> _formatters = new ();

        /// <summary>
        /// Formats a string with one argument.
        /// </summary>
        /// <typeparam name="T">Type of the single argument.</typeparam>
        /// <param name="format">A classic .NET format string as used with <see cref="string.Format(string,object?)"  />.</param>
        /// <param name="arg">An argument to use in the formatting operation.</param>
        /// <returns>The formatted string.</returns>
        public static string Format<T>(string format, T arg)
        {
            return GetFormatter(format).Format(null, arg);
        }

        /// <summary>
        /// Formats a string with one argument.
        /// </summary>
        /// <typeparam name="T">Type of the single argument.</typeparam>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="format">A classic .NET format string as used with <see cref="string.Format(string,object?)"  />.</param>
        /// <param name="arg">An argument to use in the formatting operation.</param>
        /// <returns>The formatted string.</returns>
        public static string Format<T>(IFormatProvider? provider, string format, T arg)
        {
            return GetFormatter(format).Format(provider, arg);
        }

        /// <summary>
        /// Formats a string with two arguments.
        /// </summary>
        /// <typeparam name="T0">Type of the first argument.</typeparam>
        /// <typeparam name="T1">Type of the second argument.</typeparam>
        /// <param name="format">A classic .NET format string as used with <see cref="string.Format(string,object?)"  />.</param>
        /// <param name="arg0">First argument to use in the formatting operation.</param>
        /// <param name="arg1">Second argument to use in the formatting operation.</param>
        /// <returns>The formatted string.</returns>
        public static string Format<T0, T1>(string format, T0 arg0, T1 arg1)
        {
            return GetFormatter(format).Format(null, arg0, arg1);
        }

        /// <summary>
        /// Formats a string with two arguments.
        /// </summary>
        /// <typeparam name="T0">Type of the first argument.</typeparam>
        /// <typeparam name="T1">Type of the second argument.</typeparam>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="format">A classic .NET format string as used with <see cref="string.Format(string,object?)"  />.</param>
        /// <param name="arg0">First argument to use in the formatting operation.</param>
        /// <param name="arg1">Second argument to use in the formatting operation.</param>
        /// <returns>The formatted string.</returns>
        public static string Format<T0, T1>(IFormatProvider? provider, string format, T0 arg0, T1 arg1)
        {
            return GetFormatter(format).Format(provider, arg0, arg1);
        }

        /// <summary>
        /// Formats a string with three arguments.
        /// </summary>
        /// <typeparam name="T0">Type of the first argument.</typeparam>
        /// <typeparam name="T1">Type of the second argument.</typeparam>
        /// <typeparam name="T2">Type of the third argument.</typeparam>
        /// <param name="format">A classic .NET format string as used with <see cref="string.Format(string,object?)"  />.</param>
        /// <param name="arg0">First argument to use in the formatting operation.</param>
        /// <param name="arg1">Second argument to use in the formatting operation.</param>
        /// <param name="arg2">Third argument to use in the formatting operation.</param>
        /// <returns>The formatted string.</returns>
        public static string Format<T0, T1, T2>(string format, T0 arg0, T1 arg1, T2 arg2)
        {
            return GetFormatter(format).Format(null, arg0, arg1, arg2);
        }

        /// <summary>
        /// Formats a string with three arguments.
        /// </summary>
        /// <typeparam name="T0">Type of the first argument.</typeparam>
        /// <typeparam name="T1">Type of the second argument.</typeparam>
        /// <typeparam name="T2">Type of the third argument.</typeparam>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="format">A classic .NET format string as used with <see cref="string.Format(string,object?)"  />.</param>
        /// <param name="arg0">First argument to use in the formatting operation.</param>
        /// <param name="arg1">Second argument to use in the formatting operation.</param>
        /// <param name="arg2">Third argument to use in the formatting operation.</param>
        /// <returns>The formatted string.</returns>
        public static string Format<T0, T1, T2>(IFormatProvider? provider, string format, T0 arg0, T1 arg1, T2 arg2)
        {
            return GetFormatter(format).Format(provider, arg0, arg1, arg2);
        }

        /// <summary>
        /// Formats a string with arguments.
        /// </summary>
        /// <typeparam name="T0">Type of the first argument.</typeparam>
        /// <typeparam name="T1">Type of the second argument.</typeparam>
        /// <typeparam name="T2">Type of the third argument.</typeparam>
        /// <param name="format">A classic .NET format string as used with <see cref="string.Format(string,object?)"  />.</param>
        /// <param name="arg0">First argument to use in the formatting operation.</param>
        /// <param name="arg1">Second argument to use in the formatting operation.</param>
        /// <param name="arg2">Third argument to use in the formatting operation.</param>
        /// <param name="args">Additional arguments to use in the formatting operation.</param>
        /// <returns>The formatted string.</returns>
        public static string Format<T0, T1, T2>(string format, T0 arg0, T1 arg1, T2 arg2, params object?[]? args)
        {
            return GetFormatter(format).Format(null, arg0, arg1, arg2, args);
        }

        /// <summary>
        /// Formats a string with arguments.
        /// </summary>
        /// <typeparam name="T0">Type of the first argument.</typeparam>
        /// <typeparam name="T1">Type of the second argument.</typeparam>
        /// <typeparam name="T2">Type of the third argument.</typeparam>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="format">A classic .NET format string as used with <see cref="string.Format(string,object?)"  />.</param>
        /// <param name="arg0">First argument to use in the formatting operation.</param>
        /// <param name="arg1">Second argument to use in the formatting operation.</param>
        /// <param name="arg2">Third argument to use in the formatting operation.</param>
        /// <param name="args">Additional arguments to use in the formatting operation.</param>
        /// <returns>The formatted string.</returns>
        public static string Format<T0, T1, T2>(IFormatProvider? provider, string format, T0 arg0, T1 arg1, T2 arg2, params object?[]? args)
        {
            return GetFormatter(format).Format(provider, arg0, arg1, arg2, args);
        }

        /// <summary>
        /// Formats a string with arguments.
        /// </summary>
        /// <param name="format">A classic .NET format string as used with <see cref="string.Format(string,object?)"  />.</param>
        /// <param name="args">Arguments to use in the formatting operation.</param>
        /// <returns>The formatted string.</returns>
        public static string Format(string format, params object?[]? args)
        {
            return GetFormatter(format).Format(null, args);
        }

        /// <summary>
        /// Formats a string with arguments.
        /// </summary>
        /// <param name="provider">An optional format provider that provides formatting functionality for individual arguments.</param>
        /// <param name="format">A classic .NET format string as used with <see cref="string.Format(string,object?)"  />.</param>
        /// <param name="args">Arguments to use in the formatting operation.</param>
        /// <returns>The formatted string.</returns>
        public static string Format(IFormatProvider? provider, string format, params object?[]? args)
        {
            return GetFormatter(format).Format(provider, args);
        }

        private static StringFormatter GetFormatter(string format)
        {
#pragma warning disable CPR123 // ConcurrentDictionary Count, ToArray(), CopyTo() and Clear() take locks and defeats the benefits of the concurrency.
            if (_formatters.Count >= MaxCacheEntries)
#pragma warning restore CPR123 // ConcurrentDictionary Count, ToArray(), CopyTo() and Clear() take locks and defeats the benefits of the concurrency.
            {
                return new StringFormatter(format);
            }

            return _formatters.GetOrAdd(format, key => new StringFormatter(format));
        }
    }
}

#endif
