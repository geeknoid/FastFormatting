// © Microsoft Corporation. All rights reserved.

namespace System
{
    internal readonly struct ParamsArray<T0, T1, T2>
    {
        private readonly T0? _arg0;
        private readonly T1? _arg1;
        private readonly T2? _arg2;
        private readonly object?[] _args;
        private readonly int _length;

        public ParamsArray(T0? arg0)
        {
            _arg0 = arg0;
            _arg1 = default;
            _arg2 = default;
            _args = Array.Empty<object>();
            _length = 1;
        }

        public ParamsArray(T0? arg0, T1? arg1)
        {
            _arg0 = arg0;
            _arg1 = arg1;
            _arg2 = default;
            _args = Array.Empty<object>();
            _length = 2;
        }

        public ParamsArray(T0? arg0, T1? arg1, T2? arg2)
        {
            _arg0 = arg0;
            _arg1 = arg1;
            _arg2 = arg2;
            _args = Array.Empty<object>();
            _length = 3;
        }

        public ParamsArray(T0? arg0, T1? arg1, T2? arg2, object?[] args)
        {
            _arg0 = arg0;
            _arg1 = arg1;
            _arg2 = arg2;
            _args = args;
            _length = args.Length + 3;
        }

        public T0? Arg0 => _arg0;
        public T1? Arg1 => _arg1;
        public T2? Arg2 => _arg2;
        public object?[] Args => _args;
        public int Length => _length;
    }
}
