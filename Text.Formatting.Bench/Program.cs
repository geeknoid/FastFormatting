// © Microsoft Corporation. All rights reserved.

using System;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.Emit;

#pragma warning disable CPR126 // string.Format and StringBuilder.AppendFormat are not efficient for concatenation.
#pragma warning disable S4056 // Overloads with a "CultureInfo" or an "IFormatProvider" parameter should be used
#pragma warning disable CA1822 // Mark members as static

namespace Text.Formatting.Bench
{
    [MemoryDiagnoser]
    public class Program
    {
        private const int _iterations = 100000;
        private const int _fourtyTwo = 42;
        private const string _hello = "Hello";

        private static readonly CompositeFormat _cf = new ("{0} Some literal portion in the middle {1} {2}");
        private static readonly string[] ClassicStringFormatResults = new string[_iterations];
        private static readonly string[] InterpolationResults = new string[_iterations];
        private static readonly string[] CompositeFormatResults = new string[_iterations];
        private static readonly char[] CompositeFormatWithSpanResults = new char[1024];
        private static readonly string[] StringBuilderResults = new string[_iterations];
        private static readonly char[] StringMakerBuffer = new char[1024];
        private static readonly StringBuilder Builder = new StringBuilder(1024);

        public static void Main(string[] args)
        {
            var dontRequireSlnToRunBenchmarks = ManualConfig
                .Create(DefaultConfig.Instance)
                .AddJob(Job.ShortRun.WithToolchain(InProcessEmitToolchain.Instance));

            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, dontRequireSlnToRunBenchmarks);
        }

        [Benchmark]
        public void ClassicStringFormat()
        {
            for (int i = 0; i < _iterations; i++)
            {
                ClassicStringFormatResults[i] = string.Format(null, "{0} Some literal portion in the middle {1} {2}", _hello, _fourtyTwo, i);
            }
        }

        [Benchmark]
        public void Interpolation()
        {
            for (int i = 0; i < _iterations; i++)
            {
                InterpolationResults[i] = $"{_hello} Some literal portion in the middle {_fourtyTwo} {i}";
            }
        }

        [Benchmark]
        public void StringBuilder()
        {
            for (int i = 0; i < _iterations; i++)
            {
                Builder.Clear();
                _ = Builder.AppendFormat("{0} Some literal portion in the middle {1} {2}", _hello, _fourtyTwo, i);
                StringBuilderResults[i] = Builder.ToString();
            }
        }

        [Benchmark]
        public void CompositeFormat()
        {
            for (int i = 0; i < _iterations; i++)
            {
                CompositeFormatResults[i] = _cf.Format(null, _hello, _fourtyTwo, i);
            }
        }

        [Benchmark]
        public void CompositeFormatWithSpan()
        {
            for (int i = 0; i < _iterations; i++)
            {
                _ = _cf.TryFormat(CompositeFormatWithSpanResults.AsSpan(), out int charsWritten, null, _hello, _fourtyTwo, i);
            }
        }

        [Benchmark]
        public void StringMaker()
        {
            Span<char> span = stackalloc char[128];
            for (int i = 0; i < _iterations; i++)
            {
                using var sm = new StringMaker(span);
                sm.Append(_hello, 0);
                sm.Append(" Some literal portion in the middle ", 0);
                sm.Append(_fourtyTwo, 0);
                sm.Append(" ", 0);
                sm.Append(i, 0);
                _ = sm.ExtractString();
            }
        }

        [Benchmark]
        public void StringMakerWithSpan()
        {
            for (int i = 0; i < _iterations; i++)
            {
                using var sm = new StringMaker(StringMakerBuffer);
                sm.Append(_hello, 0);
                sm.Append(" Some literal portion in the middle ", 0);
                sm.Append(_fourtyTwo, 0);
                sm.Append(" ", 0);
                sm.Append(i, 0);
                _ = sm.ExtractSpan();
            }
        }
    }
}
