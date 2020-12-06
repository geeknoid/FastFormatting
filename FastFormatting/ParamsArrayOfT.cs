// © Microsoft Corporation. All rights reserved.

namespace FastFormatting
{
    using System;

    /// <summary>
    /// Non-boxing holder of multiple parameters
    /// </summary>
    /// <remarks>
    /// Holds 3 values in strongly-typed storage, along with extra arguments as a span of objects.
    /// </remarks>
    internal readonly ref struct ParamsArray<T0, T1, T2>
    {
        public const int StronglyTypedArgs = 3;

        private readonly T0 _arg0;
        private readonly T1 _arg1;
        private readonly T2 _arg2;
        private readonly ReadOnlySpan<object?> _args;

        public ParamsArray(T0 arg0, T1 arg1, T2 arg2)
        {
            _arg0 = arg0;
            _arg1 = arg1;
            _arg2 = arg2;
            _args = Array.Empty<object>();
        }

        public ParamsArray(T0 arg0, T1 arg1, T2 arg2, ReadOnlySpan<object?> args)
        {
            _arg0 = arg0;
            _arg1 = arg1;
            _arg2 = arg2;
            _args = args;
        }

        public T0 Arg0 => _arg0;
        public T1 Arg1 => _arg1;
        public T2 Arg2 => _arg2;
        public ReadOnlySpan<object?> Args => _args;
    }
}
