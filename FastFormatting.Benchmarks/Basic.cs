// © Microsoft Corporation. All rights reserved.

namespace FastFormatting.Benchmarks
{
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Running;
    using System;

    [MemoryDiagnoser]
    public class Bench
    {
        static readonly StringFormatter _sf = new("{0} Some literal portion in the middle {1} {2}");
        const int Iterations = 100000;

        public static readonly string[] ClassicStringFormatResults = new string[Iterations];
        public static readonly string[] InterpolationResults = new string[Iterations];
        public static readonly string[] StringFormatterResults = new string[Iterations];
        public static readonly char[] StringFormatterWithSpanResults = new char[1024];
        public static string Hello = "Hello";
        public static int FourtyTwo = 42;

        [Benchmark]
        public void TestClassicStringFormat()
        {
            for (int i = 0; i < Iterations; i++)
            {
                ClassicStringFormatResults[i] = string.Format(null, "{0} Some literal portion in the middle {1} {2}", Hello, FourtyTwo, i);
            }
        }

        [Benchmark]
        public void TestInterpolation()
        {
            for (int i = 0; i < Iterations; i++)
            {
                InterpolationResults[i] = $"{Hello} Some literal portion in the middle {FourtyTwo} {i}";
            }
        }

        [Benchmark]
        public void TestStringFormatter()
        {
            for (int i = 0; i < Iterations; i++)
            {
                StringFormatterResults[i] = _sf.Format(null, Hello, FourtyTwo, i);
            }
        }

        [Benchmark]
        public void TestStringFormatterWithSpan()
        {
            for (int i = 0; i < Iterations; i++)
            {
                _ = _sf.TryFormat(StringFormatterWithSpanResults.AsSpan(), out int charsWritten, null, Hello, FourtyTwo, i);
            }
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Bench>();
        }
    }
}
