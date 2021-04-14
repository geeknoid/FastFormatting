// © Microsoft Corporation. All rights reserved.

using System;
using System.Text;

namespace Text
{
    public readonly partial struct CompositeFormat
    {
        internal StringBuilder AppendFormat<T>(StringBuilder sb, IFormatProvider? provider, T arg)
        {
            CheckNumArgs(1, null);
            return AppendFmt<T, object?, object?>(sb, provider, arg, null, null, null, EstimateArgSize(arg));
        }

        internal StringBuilder AppendFormat<T0, T1>(StringBuilder sb, IFormatProvider? provider, T0 arg0, T1 arg1)
        {
            CheckNumArgs(2, null);
            return AppendFmt<T0, T1, object?>(sb, provider, arg0, arg1, null, null, EstimateArgSize(arg0) + EstimateArgSize(arg1));
        }

        internal StringBuilder AppendFormat<T0, T1, T2>(StringBuilder sb, IFormatProvider? provider, T0 arg0, T1 arg1, T2 arg2)
        {
            CheckNumArgs(3, null);
            return AppendFmt<T0, T1, T2>(sb, provider, arg0, arg1, arg2, null, EstimateArgSize(arg0) + EstimateArgSize(arg1) + EstimateArgSize(arg2));
        }

        internal StringBuilder AppendFormat<T0, T1, T2>(StringBuilder sb, IFormatProvider? provider, T0 arg0, T1 arg1, T2 arg2, params object?[]? args)
        {
            CheckNumArgs(3, args);
            return AppendFmt<T0, T1, T2>(sb, provider, arg0, arg1, arg2, args, EstimateArgSize(arg0) + EstimateArgSize(arg1) + EstimateArgSize(arg2) + EstimateArgSize(args));
        }

        internal StringBuilder AppendFormat(StringBuilder sb, IFormatProvider? provider, params object?[]? args)
        {
            CheckNumArgs(0, args);

            if (NumArgumentsNeeded == 0)
            {
                return sb.Append(LiteralString);
            }

            var estimatedSize = EstimateArgSize(args);

            return args!.Length switch
            {
                1 => AppendFmt<object?, object?, object?>(sb, provider, args[0], null, null, null, estimatedSize),
                2 => AppendFmt<object?, object?, object?>(sb, provider, args[0], args[1], null, null, estimatedSize),
                3 => AppendFmt<object?, object?, object?>(sb, provider, args[0], args[1], args[2], null, estimatedSize),
                _ => AppendFmt<object?, object?, object?>(sb, provider, args[0], args[1], args[2], args.AsSpan(3), estimatedSize),
            };
        }

        private StringBuilder AppendFmt<T0, T1, T2>(StringBuilder sb, IFormatProvider? provider, T0 arg0, T1 arg1, T2 arg2, ReadOnlySpan<object?> args, int estimatedSize)
        {
            estimatedSize += LiteralString.Length;
            var sm = (estimatedSize >= MaxStackAlloc) ? new StringMaker(estimatedSize) : new StringMaker(stackalloc char[MaxStackAlloc]);
            CoreFmt(ref sm, provider, arg0, arg1, arg2, args);
            sm.AppendTo(sb);
            sm.Dispose();
            return sb;
        }
    }
}
