// © Microsoft Corporation. All rights reserved.

using System;
using System.Collections.Concurrent;

namespace FastFormatting
{
    readonly partial struct StringFormatter
    {
        // TODO: Perhaps this number should be tunable by the user?
        private const int MaxCacheEntries = 128;

        private static readonly ConcurrentDictionary<string, StringFormatter> _formatters = new();

        private static StringFormatter GetFormatter(string format)
        {
            if (_formatters.Count >= MaxCacheEntries)
            {
                return new StringFormatter(format);
            }

            return _formatters.GetOrAdd(format, key => new StringFormatter(format));
        }

        public static string Format<T>(string format, T arg)
        {
            return GetFormatter(format).Format(null, arg);
        }

        public static string Format<T>(IFormatProvider? provider, string format, T arg)
        {
            return GetFormatter(format).Format(provider, arg);
        }

        public static string Format<T0, T1>(string format, T0 arg0, T1 arg1)
        {
            return GetFormatter(format).Format(null, arg0, arg1);
        }

        public static string Format<T0, T1>(IFormatProvider? provider, string format, T0 arg0, T1 arg1)
        {
            return GetFormatter(format).Format(provider, arg0, arg1);
        }

        public static string Format<T0, T1, T2>(string format, T0 arg0, T1 arg1, T2 arg2)
        {
            return GetFormatter(format).Format(null, arg0, arg1, arg2);
        }

        public static string Format<T0, T1, T2>(IFormatProvider? provider, string format, T0 arg0, T1 arg1, T2 arg2)
        {
            return GetFormatter(format).Format(provider, arg0, arg1, arg2);
        }

        public static string Format<T0, T1, T2>(string format, T0 arg0, T1 arg1, T2 arg2, params object?[]? args)
        {
            return GetFormatter(format).Format(null, arg0, arg1, arg2, args);
        }

        public static string Format<T0, T1, T2>(IFormatProvider? provider, string format, T0 arg0, T1 arg1, T2 arg2, params object?[]? args)
        {
            return GetFormatter(format).Format(provider, arg0, arg1, arg2, args);
        }

        public static string Format(string format, params object?[]? args)
        {
            return GetFormatter(format).Format(null, args);
        }

        public static string Format(IFormatProvider? provider, string format, params object?[]? args)
        {
            return GetFormatter(format).Format(provider, args);
        }
    }
}
