// © Microsoft Corporation. All rights reserved.

namespace FastFormatting.Benchmarks
{
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Running;
    using System;
    using System.Text;

    [MemoryDiagnoser]
    public class Bench
    {
        static readonly StringFormatter _sf = new("{0} Some literal portion in the middle {1} {2}");
        const int Iterations = 100000;

        public static readonly string[] StringFormatResults = new string[Iterations];
        public static readonly string[] StringFormatterResults = new string[Iterations];
        public static readonly string[] InterpolationResults = new string[Iterations];
        public static string Hello = "Hello";
        public static int FourtyTwo = 42;

        [Benchmark]
        public void TestStringFormat()
        {
            for (int i = 0; i < Iterations; i++)
            {
                StringFormatResults[i] = string.Format(null, "{0} Some literal portion in the middle {1} {2}", Hello, FourtyTwo, i);
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
        public void TestInterpolation()
        {
            for (int i = 0; i < Iterations; i++)
            {
                InterpolationResults[i] = $"{Hello} Some literal portion in the middle {FourtyTwo} {i}";
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
