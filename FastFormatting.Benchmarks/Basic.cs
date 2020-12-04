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

        [Benchmark]
        public void TestStringFormat()
        {
            for (int i = 0; i < 100000; i++)
            {
                String.Format(null, "{0} Some literal portion in the middle {1} {2}", "Hello", "World", 42);
            }
        }

        [Benchmark]
        public void TestStringFormatter()
        {
            for (int i = 0; i < 100000; i++)
            {
                _sf.Format(null, "Hello", "World", 42);
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
