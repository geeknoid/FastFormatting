// © Microsoft Corporation. All rights reserved.

using System;

namespace Text
{
    /// <summary>
    /// Non-boxing holder of multiple parameters.
    /// </summary>
    /// <remarks>
    /// Holds 3 values in strongly-typed storage, along with extra arguments as a span of objects.
    /// </remarks>
    internal readonly ref struct Params<T0, T1, T2>
    {
        public Params(T0 arg0, T1 arg1, T2 arg2)
        {
            Arg0 = arg0;
            Arg1 = arg1;
            Arg2 = arg2;
            Args = Array.Empty<object>();
        }

        public Params(T0 arg0, T1 arg1, T2 arg2, ReadOnlySpan<object?> args)
        {
            Arg0 = arg0;
            Arg1 = arg1;
            Arg2 = arg2;
            Args = args;
        }

        public T0 Arg0 { get; }
        public T1 Arg1 { get; }
        public T2 Arg2 { get; }
        public ReadOnlySpan<object?> Args { get; }
    }
}
