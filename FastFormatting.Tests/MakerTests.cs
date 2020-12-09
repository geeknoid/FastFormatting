// © Microsoft Corporation. All rights reserved.

using System;
using Xunit;
using Xunit.Abstractions;

namespace FastFormatting.Tests
{
    public class MakerTests
    {
        private readonly ITestOutputHelper _output;

        public MakerTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void TestExpansion_Int64()
        {
            long o = 123456;
            for (int capacity = 0; capacity < 20; capacity++)
            {
                var sm = new StringMaker(capacity);
                sm.Append(o);
                var actual = sm.ExtractString();
                var expected = String.Format("{0}", o);
                Assert.Equal(expected, actual);

                sm = new StringMaker(capacity);
                sm.Append(o);
                actual = sm.ExtractSpan().ToString();
                Assert.Equal(expected, actual);

                for (int width = -10; width < 10; width++)
                {
                    sm = new StringMaker(capacity);
                    sm.Append(o, "", null, width);
                    actual = sm.ExtractString();
                    expected = String.Format(String.Format("{{0,{0}}}", width), o);
                    Assert.Equal(expected, actual);

                    sm = new StringMaker(capacity);
                    sm.Append(o, "", null, width);
                    actual = sm.ExtractSpan().ToString();
                    Assert.Equal(expected, actual);
                }
            }

            {
                var sm = new StringMaker(new char[0], true);
                sm.Append(o);
                Assert.True(sm.Overflowed);
                Assert.Equal(string.Empty, sm.ExtractString());

                sm = new StringMaker(new char[0], true);
                sm.Append(o);
                Assert.Equal(string.Empty, sm.ExtractSpan().ToString());

                sm = new StringMaker(new char[0], true);
                sm.Append(o, string.Empty, null, 0);
                Assert.True(sm.Overflowed);
                Assert.Equal(string.Empty, sm.ExtractString());
            }
        }

        [Fact]
        public void TestExpansion_Uint64()
        {
            ulong o = 123456;
            for (int capacity = 0; capacity < 20; capacity++)
            {
                var sm = new StringMaker(capacity);
                sm.Append(o);
                var actual = sm.ExtractString();
                var expected = String.Format("{0}", o);
                Assert.Equal(expected, actual);

                for (int width = -10; width < 10; width++)
                {
                    sm = new StringMaker(capacity);
                    sm.Append(o, "", null, width);
                    actual = sm.ExtractString();
                    expected = String.Format(String.Format("{{0,{0}}}", width), o);
                    Assert.Equal(expected, actual);
                }
            }

            {
                var sm = new StringMaker(new char[0], true);
                sm.Append(o);
                Assert.True(sm.Overflowed);
                Assert.Equal(string.Empty, sm.ExtractString());

                sm = new StringMaker(new char[0], true);
                sm.Append(o);
                Assert.Equal(string.Empty, sm.ExtractSpan().ToString());

                sm = new StringMaker(new char[0], true);
                sm.Append(o, string.Empty, null, 0);
                Assert.True(sm.Overflowed);
                Assert.Equal(string.Empty, sm.ExtractString());
            }
        }

        [Fact]
        public void TestExpansion_Double()
        {
            double o = 123.456;
            for (int capacity = 0; capacity < 20; capacity++)
            {
                var sm = new StringMaker(capacity);
                sm.Append(o);
                var actual = sm.ExtractString();
                var expected = String.Format("{0}", o);
                Assert.Equal(expected, actual);

                for (int width = -10; width < 10; width++)
                {
                    sm = new StringMaker(capacity);
                    sm.Append(o, "", null, width);
                    actual = sm.ExtractString();
                    expected = String.Format(String.Format("{{0,{0}}}", width), o);
                    Assert.Equal(expected, actual);
                }
            }

            {
                var sm = new StringMaker(new char[0], true);
                sm.Append(o);
                Assert.True(sm.Overflowed);
                Assert.Equal(string.Empty, sm.ExtractString());

                sm = new StringMaker(new char[0], true);
                sm.Append(o);
                Assert.Equal(string.Empty, sm.ExtractSpan().ToString());

                sm = new StringMaker(new char[0], true);
                sm.Append(o, string.Empty, null, 0);
                Assert.True(sm.Overflowed);
                Assert.Equal(string.Empty, sm.ExtractString());
            }
        }

        [Fact]
        public void TestExpansion_Bool()
        {
            bool o = true;
            for (int capacity = 0; capacity < 20; capacity++)
            {
                var sm = new StringMaker(capacity);
                sm.Append(o);
                var actual = sm.ExtractString();
                var expected = String.Format("{0}", o);
                Assert.Equal(expected, actual);

                for (int width = -10; width < 10; width++)
                {
                    sm = new StringMaker(capacity);
                    sm.Append(o, width);
                    actual = sm.ExtractString();
                    expected = String.Format(String.Format("{{0,{0}}}", width), o);
                    Assert.Equal(expected, actual);
                }
            }

            {
                var sm = new StringMaker(new char[0], true);
                sm.Append(o);
                Assert.True(sm.Overflowed);
                Assert.Equal(string.Empty, sm.ExtractString());

                sm = new StringMaker(new char[0], true);
                sm.Append(o);
                Assert.Equal(string.Empty, sm.ExtractSpan().ToString());

                sm = new StringMaker(new char[0], true);
                sm.Append(o, 0);
                Assert.True(sm.Overflowed);
                Assert.Equal(string.Empty, sm.ExtractString());
            }
        }

        [Fact]
        public void TestExpansion_Decimal()
        {
            decimal o = new decimal(123.456);
            for (int capacity = 0; capacity < 20; capacity++)
            {
                var sm = new StringMaker(capacity);
                sm.Append(o);
                var actual = sm.ExtractString();
                var expected = String.Format("{0}", o);
                Assert.Equal(expected, actual);

                for (int width = -10; width < 10; width++)
                {
                    sm = new StringMaker(capacity);
                    sm.Append(o, "", null, width);
                    actual = sm.ExtractString();
                    expected = String.Format(String.Format("{{0,{0}}}", width), o);
                    Assert.Equal(expected, actual);
                }
            }

            {
                var sm = new StringMaker(new char[0], true);
                sm.Append(o);
                Assert.True(sm.Overflowed);
                Assert.Equal(string.Empty, sm.ExtractString());

                sm = new StringMaker(new char[0], true);
                sm.Append(o);
                Assert.Equal(string.Empty, sm.ExtractSpan().ToString());

                sm = new StringMaker(new char[0], true);
                sm.Append(o, string.Empty, null, 0);
                Assert.True(sm.Overflowed);
                Assert.Equal(string.Empty, sm.ExtractString());
            }
        }

        [Fact]
        public void TestExpansion_DateTime()
        {
            var o = DateTime.Now;
            for (int capacity = 0; capacity < 20; capacity++)
            {
                var sm = new StringMaker(capacity);
                sm.Append(o);
                var actual = sm.ExtractString();
                var expected = String.Format("{0}", o);
                Assert.Equal(expected, actual);

                for (int width = -10; width < 10; width++)
                {
                    sm = new StringMaker(capacity);
                    sm.Append(o, "", null, width);
                    actual = sm.ExtractString();
                    expected = String.Format(String.Format("{{0,{0}}}", width), o);
                    Assert.Equal(expected, actual);
                }
            }

            {
                var sm = new StringMaker(new char[0], true);
                sm.Append(o);
                Assert.True(sm.Overflowed);
                Assert.Equal(string.Empty, sm.ExtractString());

                sm = new StringMaker(new char[0], true);
                sm.Append(o);
                Assert.Equal(string.Empty, sm.ExtractSpan().ToString());

                sm = new StringMaker(new char[0], true);
                sm.Append(o, string.Empty, null, 0);
                Assert.True(sm.Overflowed);
                Assert.Equal(string.Empty, sm.ExtractString());
            }
        }

        [Fact]
        public void TestExpansion_TimeSpan()
        {
            var o = new TimeSpan(123456);
            for (int capacity = 0; capacity < 20; capacity++)
            {
                var sm = new StringMaker(capacity);
                sm.Append(o);
                var actual = sm.ExtractString();
                var expected = String.Format("{0}", o);
                Assert.Equal(expected, actual);

                for (int width = -10; width < 10; width++)
                {
                    sm = new StringMaker(capacity);
                    sm.Append(o, "", null, width);
                    actual = sm.ExtractString();
                    expected = String.Format(String.Format("{{0,{0}}}", width), o);
                    Assert.Equal(expected, actual);
                }
            }

            {
                var sm = new StringMaker(new char[0], true);
                sm.Append(o);
                Assert.True(sm.Overflowed);
                Assert.Equal(string.Empty, sm.ExtractString());

                sm = new StringMaker(new char[0], true);
                sm.Append(o);
                Assert.Equal(string.Empty, sm.ExtractSpan().ToString());

                sm = new StringMaker(new char[0], true);
                sm.Append(o, string.Empty, null, 0);
                Assert.True(sm.Overflowed);
                Assert.Equal(string.Empty, sm.ExtractString());
            }
        }

        [Fact]
        public void TestExpansion_Char()
        {
            char o = 'x';
            for (int capacity = 0; capacity < 20; capacity++)
            {
                _output.WriteLine($"Capacity {capacity}");

                var sm = new StringMaker(capacity);
                sm.Append(o);
                var actual = sm.ExtractString();
                var expected = String.Format("{0}", o);
                Assert.Equal(expected, actual);

                for (int width = -10; width < 10; width++)
                {
                    _output.WriteLine($"  width {width}");

                    sm = new StringMaker(capacity);
                    sm.Append(o, width);
                    actual = sm.ExtractString();
                    expected = String.Format(String.Format("{{0,{0}}}", width), o);
                    Assert.Equal(expected, actual);
                }
            }

            {
                var sm = new StringMaker(new char[0], true);
                sm.Append(o);
                Assert.True(sm.Overflowed);
                Assert.Equal(string.Empty, sm.ExtractString());

                sm = new StringMaker(new char[0], true);
                sm.Append(o);
                Assert.Equal(string.Empty, sm.ExtractSpan().ToString());

                sm = new StringMaker(new char[0], true);
                sm.Append(o, string.Empty, null, 0);
                Assert.True(sm.Overflowed);
                Assert.Equal(string.Empty, sm.ExtractString());
            }
        }

        [Fact]
        public void TestExpansion_String()
        {
            var o = "123456";
            for (int capacity = 0; capacity < 20; capacity++)
            {
                var sm = new StringMaker(capacity);
                sm.Append(o);
                var actual = sm.ExtractString();
                var expected = String.Format("{0}", o);
                Assert.Equal(expected, actual);

                for (int width = -10; width < 10; width++)
                {
                    sm = new StringMaker(capacity);
                    sm.Append(o, width);
                    actual = sm.ExtractString();
                    expected = String.Format(String.Format("{{0,{0}}}", width), o);
                    Assert.Equal(expected, actual);
                }
            }

            {
                var sm = new StringMaker(new char[0], true);
                sm.Append(o);
                Assert.True(sm.Overflowed);
                Assert.Equal(string.Empty, sm.ExtractString());

                sm = new StringMaker(new char[0], true);
                sm.Append(o);
                Assert.Equal(string.Empty, sm.ExtractSpan().ToString());

                sm = new StringMaker(new char[0], true);
                sm.Append(o, 0);
                Assert.True(sm.Overflowed);
                Assert.Equal(string.Empty, sm.ExtractString());
            }
        }

        [Fact]
        public void TestExpansion_Span()
        {
            var o = "123456".AsSpan();
            for (int capacity = 0; capacity < 20; capacity++)
            {
                var sm = new StringMaker(capacity);
                sm.Append(o);
                var actual = sm.ExtractString();
                var expected = String.Format("{0}", "123456");
                Assert.Equal(expected, actual);

                for (int width = -10; width < 10; width++)
                {
                    sm = new StringMaker(capacity);
                    sm.Append(o, width);
                    actual = sm.ExtractString();
                    expected = String.Format(String.Format("{{0,{0}}}", width), "123456");
                    Assert.Equal(expected, actual);
                }
            }

            {
                var sm = new StringMaker(new char[0], true);
                sm.Append(o);
                Assert.True(sm.Overflowed);
                Assert.Equal(string.Empty, sm.ExtractString());

                sm = new StringMaker(new char[0], true);
                sm.Append(o);
                Assert.Equal(string.Empty, sm.ExtractSpan().ToString());

                sm = new StringMaker(new char[0], true);
                sm.Append(o, 0);
                Assert.True(sm.Overflowed);
                Assert.Equal(string.Empty, sm.ExtractString());
            }
        }

        [Fact]
        public void TestExpansion_Object()
        {
            var o = (object)"123456";
            for (int capacity = 0; capacity < 20; capacity++)
            {
                var sm = new StringMaker(capacity);
                sm.Append(o);
                var actual = sm.ExtractString();
                var expected = String.Format("{0}", o);
                Assert.Equal(expected, actual);

                for (int width = -10; width < 10; width++)
                {
                    sm = new StringMaker(capacity);
                    sm.Append(o, width);
                    actual = sm.ExtractString();
                    expected = String.Format(String.Format("{{0,{0}}}", width), o);
                    Assert.Equal(expected, actual);
                }
            }

            {
                var sm = new StringMaker(new char[0], true);
                sm.Append(o);
                Assert.True(sm.Overflowed);
                Assert.Equal(string.Empty, sm.ExtractString());

                sm = new StringMaker(new char[0], true);
                sm.Append(o);
                Assert.Equal(string.Empty, sm.ExtractSpan().ToString());

                sm = new StringMaker(new char[0], true);
                sm.Append(o, 0);
                Assert.True(sm.Overflowed);
                Assert.Equal(string.Empty, sm.ExtractString());
            }
        }

        struct SpanFormattable : ISpanFormattable
        {
            public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
            {
                if (destination.Length < 6)
                {
                    charsWritten = 0;
                    return false;
                }

                "123456".AsSpan().CopyTo(destination);
                charsWritten = 6;
                return true;
            }

            public override string ToString()
            {
                return "123456";
            }
        }

        [Fact]
        public void TestExpansion_T()
        {
            var o = new SpanFormattable();
            for (int capacity = 0; capacity < 20; capacity++)
            {
                _output.WriteLine($"Capacity {capacity}");

                var sm = new StringMaker(capacity);
                sm.Append(o);
                var actual = sm.ExtractString();
                var expected = String.Format("{0}", o);
                Assert.Equal(expected, actual);

                for (int width = -10; width < 10; width++)
                {
                    _output.WriteLine($"  {width}");

                    sm = new StringMaker(capacity);
                    sm.Append(o, string.Empty, null, width);
                    actual = sm.ExtractString();
                    expected = String.Format(String.Format("{{0,{0}}}", width), o);
                    Assert.Equal(expected, actual);
                }
            }

            {
                var sm = new StringMaker(new char[0], true);
                sm.Append(o);
                Assert.True(sm.Overflowed);
                Assert.Equal(string.Empty, sm.ExtractString());

                sm = new StringMaker(new char[0], true);
                sm.Append(o);
                Assert.Equal(string.Empty, sm.ExtractSpan().ToString());

                sm = new StringMaker(new char[0], true);
                sm.Append(o, string.Empty, null, 0);
                Assert.True(sm.Overflowed);
                Assert.Equal(string.Empty, sm.ExtractString());
            }
        }

        struct Formattable : IFormattable
        {
            public string ToString(string? format, IFormatProvider? formatProvider)
            {
                return "123456";
            }
        }

        [Fact]
        public void TestExpansion_Formattable()
        {
            var o = new Formattable();
            for (int capacity = 0; capacity < 20; capacity++)
            {
                _output.WriteLine($"Capacity {capacity}");

                var sm = new StringMaker(capacity);
                sm.Append(o);
                var actual = sm.ExtractString();
                var expected = String.Format("{0}", o);
                Assert.Equal(expected, actual);

                for (int width = -10; width < 10; width++)
                {
                    _output.WriteLine($"  {width}");

                    sm = new StringMaker(capacity);
                    sm.Append(o, string.Empty, null, width);
                    actual = sm.ExtractString();
                    expected = String.Format(String.Format("{{0,{0}}}", width), o);
                    Assert.Equal(expected, actual);
                }
            }

            {
                var sm = new StringMaker(new char[0], true);
                sm.Append(o);
                Assert.True(sm.Overflowed);
                Assert.Equal(string.Empty, sm.ExtractString());

                sm = new StringMaker(new char[0], true);
                sm.Append(o);
                Assert.Equal(string.Empty, sm.ExtractSpan().ToString());

                sm = new StringMaker(new char[0], true);
                sm.Append(o, string.Empty, null, 0);
                Assert.True(sm.Overflowed);
                Assert.Equal(string.Empty, sm.ExtractString());
            }
        }

        [Fact]
        public void TestNullArgs()
        {
            var sm = new StringMaker();
            sm.Append((string ?)null);
            Assert.Equal(string.Empty, sm.ExtractString());

            sm = new StringMaker();
            sm.Append((string?)null, 12);
            Assert.Equal("            ", sm.ExtractString());

            sm = new StringMaker();
            sm.Append((object?)null);
            Assert.Equal(string.Empty, sm.ExtractString());

            sm = new StringMaker();
            sm.Append((object?)null, 12);
            Assert.Equal("            ", sm.ExtractString());
        }
    }
}
