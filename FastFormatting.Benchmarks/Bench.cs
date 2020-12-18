// © Microsoft Corporation. All rights reserved.

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Text;

namespace FastFormatting.Benchmarks
{
    [MemoryDiagnoser]
    public class Bench
    {
        static readonly StringFormatter _sf = new("{0} Some literal portion in the middle {1} {2}");
        const int Iterations = 100000;

        public static readonly string[] ClassicStringFormatResults = new string[Iterations];
        public static readonly string[] InterpolationResults = new string[Iterations];
        public static readonly string[] StringFormatterResults = new string[Iterations];
        public static readonly char[] StringFormatterWithSpanResults = new char[1024];
        public static readonly string[] StringBuilderResults = new string[Iterations];
        public static string Hello = "Hello";
        public static int FourtyTwo = 42;
        public static readonly char[] StringMakerBuffer = new char[1024];
        public static readonly StringBuilder Builder = new StringBuilder(1024);

        [Benchmark]
        public void ClassicStringFormat()
        {
            for (int i = 0; i < Iterations; i++)
            {
                ClassicStringFormatResults[i] = string.Format(null, "{0} Some literal portion in the middle {1} {2}", Hello, FourtyTwo, i);
            }
        }

        [Benchmark]
        public void Interpolation()
        {
            for (int i = 0; i < Iterations; i++)
            {
                InterpolationResults[i] = $"{Hello} Some literal portion in the middle {FourtyTwo} {i}";
            }
        }

        [Benchmark]
        public void StringBuilder()
        {
            for (int i = 0; i < Iterations; i++)
            {
                Builder.Clear();
                Builder.AppendFormat("{0} Some literal portion in the middle {1} {2}", Hello, FourtyTwo, i);
                StringBuilderResults[i] = Builder.ToString();
            }
        }

        [Benchmark]
        public void StringFormatter()
        {
            for (int i = 0; i < Iterations; i++)
            {
                StringFormatterResults[i] = _sf.Format(null, Hello, FourtyTwo, i);
            }
        }

        [Benchmark]
        public void StringFormatterWithSpan()
        {
            for (int i = 0; i < Iterations; i++)
            {
                _ = _sf.TryFormat(StringFormatterWithSpanResults.AsSpan(), out int charsWritten, null, Hello, FourtyTwo, i);
            }
        }

        [Benchmark]
        public void StringMaker()
        {
            Span<char> span = stackalloc char[128];
            for (int i = 0; i < Iterations; i++)
            {
                var sm = new StringMaker(span);
                sm.Append(Hello);
                sm.Append(" Some literal portion in the middle ");
                sm.Append(FourtyTwo);
                sm.Append(" ");
                sm.Append(i);
                _ = sm.ExtractString();
            }
        }

        [Benchmark]
        public void StringMakerWithSpan()
        {
            for (int i = 0; i < Iterations; i++)
            {
                var sm = new StringMaker(StringMakerBuffer);
                sm.Append(Hello);
                sm.Append(" Some literal portion in the middle ");
                sm.Append(FourtyTwo);
                sm.Append(" ");
                sm.Append(i);
                _ = sm.ExtractSpan();
            }
        }

        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Bench>();
        }
    }
}
