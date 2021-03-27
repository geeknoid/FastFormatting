// © Microsoft Corporation. All rights reserved.

using System;
using Xunit;

#pragma warning disable S4056 // Overloads with a "CultureInfo" or an "IFormatProvider" parameter should be used
#pragma warning disable CPR126 // string.Format and StringBuilder.AppendFormat are not efficient for concatenation.

namespace Text.Formatting.Test
{
    public class MakerTests
    {
        [Fact]
        public void TestExpansion_Int64()
        {
            StringMaker sm;
            string expected;
            string actual;

            long o = 123456;
            for (int capacity = 0; capacity < 20; capacity++)
            {
#if PUBLIC_STRINGMAKER
                sm = new StringMaker(capacity);
                sm.Append(o);
                actual = sm.ExtractString();
                expected = string.Format("{0}", o);
                Assert.Equal(expected, actual);
                sm.Dispose();

                sm = new StringMaker(capacity);
                sm.Append(o);
                actual = sm.ExtractSpan().ToString();
                Assert.Equal(expected, actual);
                sm.Dispose();
#endif
                for (int width = -10; width < 10; width++)
                {
                    sm = new StringMaker(capacity);
                    sm.Append(o, "", null, width);
                    actual = sm.ExtractString();
                    expected = string.Format(string.Format("{{0,{0}}}", width), o);
                    Assert.Equal(expected, actual);
                    sm.Dispose();

                    sm = new StringMaker(capacity);
                    sm.Append(o, "", null, width);
                    actual = sm.ExtractSpan().ToString();
                    Assert.Equal(expected, actual);
                    sm.Dispose();
                }
            }

#if PUBLIC_STRINGMAKER
            sm = new StringMaker(Array.Empty<char>(), true);
            sm.Append(o);
            Assert.True(sm.Overflowed);
            Assert.Equal(string.Empty, sm.ExtractString());
            sm.Dispose();
#endif

            sm = new StringMaker(Array.Empty<char>(), true);
            sm.Append(o, string.Empty, null, 0);
            Assert.True(sm.Overflowed);
            Assert.Equal(string.Empty, sm.ExtractSpan().ToString());
            Assert.Equal(string.Empty, sm.ExtractString());
            sm.Dispose();
        }

        [Fact]
        public void TestExpansion_Uint64()
        {
            StringMaker sm;
            string expected;
            string actual;

            ulong o = 123456;
            for (int capacity = 0; capacity < 20; capacity++)
            {
#if PUBLIC_STRINGMAKER
                sm = new StringMaker(capacity);
                sm.Append(o);
                actual = sm.ExtractString();
                expected = string.Format("{0}", o);
                Assert.Equal(expected, actual);
                sm.Dispose();
#endif

                for (int width = -10; width < 10; width++)
                {
                    sm = new StringMaker(capacity);
                    sm.Append(o, "", null, width);
                    actual = sm.ExtractString();
                    expected = string.Format(string.Format("{{0,{0}}}", width), o);
                    Assert.Equal(expected, actual);
                    sm.Dispose();
                }
            }

#if PUBLIC_STRINGMAKER
            sm = new StringMaker(Array.Empty<char>(), true);
            sm.Append(o);
            Assert.True(sm.Overflowed);
            Assert.Equal(string.Empty, sm.ExtractString());
            sm.Dispose();
#endif

            sm = new StringMaker(Array.Empty<char>(), true);
            sm.Append(o, string.Empty, null, 0);
            Assert.True(sm.Overflowed);
            Assert.Equal(string.Empty, sm.ExtractSpan().ToString());
            Assert.Equal(string.Empty, sm.ExtractString());
            sm.Dispose();

            sm = new StringMaker(new char[1], true);
            sm.Append(1, string.Empty, null, 2);
            Assert.True(sm.Overflowed);
            Assert.Equal("1", sm.ExtractString());
            sm.Dispose();
        }

        [Fact]
        public void TestExpansion_Double()
        {
            StringMaker sm;
            string expected;
            string actual;

            double o = 123.456;
            for (int capacity = 0; capacity < 20; capacity++)
            {
#if PUBLIC_STRINGMAKER
                sm = new StringMaker(capacity);
                sm.Append(o);
                actual = sm.ExtractString();
                expected = string.Format("{0}", o);
                Assert.Equal(expected, actual);
                sm.Dispose();
#endif

                for (int width = -10; width < 10; width++)
                {
                    sm = new StringMaker(capacity);
                    sm.Append(o, "", null, width);
                    actual = sm.ExtractString();
                    expected = string.Format(string.Format("{{0,{0}}}", width), o);
                    Assert.Equal(expected, actual);
                    sm.Dispose();
                }
            }

#if PUBLIC_STRINGMAKER
            sm = new StringMaker(Array.Empty<char>(), true);
            sm.Append(o);
            Assert.True(sm.Overflowed);
            Assert.Equal(string.Empty, sm.ExtractString());
            sm.Dispose();
#endif

            sm = new StringMaker(Array.Empty<char>(), true);
            sm.Append(o, string.Empty, null, 0);
            Assert.True(sm.Overflowed);
            Assert.Equal(string.Empty, sm.ExtractSpan().ToString());
            Assert.Equal(string.Empty, sm.ExtractString());
            sm.Dispose();
        }

        [Fact]
        public void TestExpansion_Bool()
        {
            StringMaker sm;
            string expected;
            string actual;

            bool o = true;
            for (int capacity = 0; capacity < 20; capacity++)
            {
#if PUBLIC_STRINGMAKER
                sm = new StringMaker(capacity);
                sm.Append(o);
                actual = sm.ExtractString();
                expected = string.Format("{0}", o);
                Assert.Equal(expected, actual);
                sm.Dispose();
#endif

                for (int width = -10; width < 10; width++)
                {
                    sm = new StringMaker(capacity);
                    sm.Append(o, width);
                    actual = sm.ExtractString();
                    expected = string.Format(string.Format("{{0,{0}}}", width), o);
                    Assert.Equal(expected, actual);
                    sm.Dispose();
                }
            }

#if PUBLIC_STRINGMAKER
            sm = new StringMaker(Array.Empty<char>(), true);
            sm.Append(o);
            Assert.True(sm.Overflowed);
            Assert.Equal(string.Empty, sm.ExtractString());
            sm.Dispose();
#endif

            sm = new StringMaker(Array.Empty<char>(), true);
            sm.Append(o, 0);
            Assert.True(sm.Overflowed);
            Assert.Equal(string.Empty, sm.ExtractSpan().ToString());
            Assert.Equal(string.Empty, sm.ExtractString());
            sm.Dispose();
        }

        [Fact]
        public void TestExpansion_Decimal()
        {
            StringMaker sm;
            string expected;
            string actual;

            decimal o = new decimal(123.456);
            for (int capacity = 0; capacity < 20; capacity++)
            {
#if PUBLIC_STRINGMAKER
                sm = new StringMaker(capacity);
                sm.Append(o);
                actual = sm.ExtractString();
                expected = string.Format("{0}", o);
                Assert.Equal(expected, actual);
                sm.Dispose();
#endif

                for (int width = -10; width < 10; width++)
                {
                    sm = new StringMaker(capacity);
                    sm.Append(o, "", null, width);
                    actual = sm.ExtractString();
                    expected = string.Format(string.Format("{{0,{0}}}", width), o);
                    Assert.Equal(expected, actual);
                    sm.Dispose();
                }
            }

#if PUBLIC_STRINGMAKER
            sm = new StringMaker(Array.Empty<char>(), true);
            sm.Append(o);
            Assert.True(sm.Overflowed);
            Assert.Equal(string.Empty, sm.ExtractString());
            sm.Dispose();
#endif

            sm = new StringMaker(Array.Empty<char>(), true);
            sm.Append(o, string.Empty, null, 0);
            Assert.True(sm.Overflowed);
            Assert.Equal(string.Empty, sm.ExtractSpan().ToString());
            Assert.Equal(string.Empty, sm.ExtractString());
            sm.Dispose();
        }

        [Fact]
        public void TestExpansion_DateTime()
        {
            StringMaker sm;
            string expected;
            string actual;

            var o = DateTime.UtcNow;
            for (int capacity = 0; capacity < 20; capacity++)
            {
#if PUBLIC_STRINGMAKER
                sm = new StringMaker(capacity);
                sm.Append(o);
                actual = sm.ExtractString();
                expected = string.Format("{0}", o);
                Assert.Equal(expected, actual);
                sm.Dispose();
#endif
                for (int width = -10; width < 10; width++)
                {
                    sm = new StringMaker(capacity);
                    sm.Append(o, "", null, width);
                    actual = sm.ExtractString();
                    expected = string.Format(string.Format("{{0,{0}}}", width), o);
                    Assert.Equal(expected, actual);
                    sm.Dispose();
                }
            }

#if PUBLIC_STRINGMAKER
            sm = new StringMaker(Array.Empty<char>(), true);
            sm.Append(o);
            Assert.True(sm.Overflowed);
            Assert.Equal(string.Empty, sm.ExtractString());
            sm.Dispose();
#endif

            sm = new StringMaker(Array.Empty<char>(), true);
            sm.Append(o, string.Empty, null, 0);
            Assert.True(sm.Overflowed);
            Assert.Equal(string.Empty, sm.ExtractSpan().ToString());
            Assert.Equal(string.Empty, sm.ExtractString());
            sm.Dispose();
        }

        [Fact]
        public void TestExpansion_TimeSpan()
        {
            StringMaker sm;
            string expected;
            string actual;

            var o = new TimeSpan(123456);
            for (int capacity = 0; capacity < 20; capacity++)
            {
#if PUBLIC_STRINGMAKER
                sm = new StringMaker(capacity);
                sm.Append(o);
                actual = sm.ExtractString();
                expected = string.Format("{0}", o);
                Assert.Equal(expected, actual);
                sm.Dispose();
#endif

                for (int width = -10; width < 10; width++)
                {
                    sm = new StringMaker(capacity);
                    sm.Append(o, "", null, width);
                    actual = sm.ExtractString();
                    expected = string.Format(string.Format("{{0,{0}}}", width), o);
                    Assert.Equal(expected, actual);
                    sm.Dispose();
                }
            }

#if PUBLIC_STRINGMAKER
            sm = new StringMaker(Array.Empty<char>(), true);
            sm.Append(o);
            Assert.True(sm.Overflowed);
            Assert.Equal(string.Empty, sm.ExtractString());
            sm.Dispose();
#endif

            sm = new StringMaker(Array.Empty<char>(), true);
            sm.Append(o, string.Empty, null, 0);
            Assert.True(sm.Overflowed);
            Assert.Equal(string.Empty, sm.ExtractSpan().ToString());
            Assert.Equal(string.Empty, sm.ExtractString());
            sm.Dispose();
        }

        [Fact]
        public void TestExpansion_Char()
        {
            StringMaker sm;
            string expected;
            string actual;

            char o = 'x';
            for (int capacity = 0; capacity < 20; capacity++)
            {
                sm = new StringMaker(capacity);
                sm.Append(o);
                actual = sm.ExtractString();
                expected = string.Format("{0}", o);
                Assert.Equal(expected, actual);
                sm.Dispose();

                for (int width = -10; width < 10; width++)
                {
                    sm = new StringMaker(capacity);
                    sm.Append(o, width);
                    actual = sm.ExtractString();
                    expected = string.Format(string.Format("{{0,{0}}}", width), o);
                    Assert.Equal(expected, actual);
                    sm.Dispose();
                }
            }

            sm = new StringMaker(Array.Empty<char>(), true);
            sm.Append(o);
            Assert.True(sm.Overflowed);
            Assert.Equal(string.Empty, sm.ExtractString());
            sm.Dispose();

            sm = new StringMaker(Array.Empty<char>(), true);
            sm.Append(o, 0);
            Assert.True(sm.Overflowed);
            Assert.Equal(string.Empty, sm.ExtractSpan().ToString());
            Assert.Equal(string.Empty, sm.ExtractString());
            sm.Dispose();

            sm = new StringMaker(Array.Empty<char>(), true);
            sm.Append(o, 2);
            Assert.True(sm.Overflowed);
            Assert.Equal(string.Empty, sm.ExtractString());
            sm.Dispose();

            sm = new StringMaker(Array.Empty<char>(), true);
            sm.Append(o, -2);
            Assert.True(sm.Overflowed);
            Assert.Equal(string.Empty, sm.ExtractString());
            sm.Dispose();
        }

        [Fact]
        public void TestExpansion_String()
        {
            StringMaker sm;
            string expected;
            string actual;

            var o = "123456";
            for (int capacity = 0; capacity < 20; capacity++)
            {
#if PUBLIC_STRINGMAKER
                sm = new StringMaker(capacity);
                sm.Append(o);
                actual = sm.ExtractString();
                expected = string.Format("{0}", o);
                Assert.Equal(expected, actual);
                sm.Dispose();
#endif
                for (int width = -10; width < 10; width++)
                {
                    sm = new StringMaker(capacity);
                    sm.Append(o, width);
                    actual = sm.ExtractString();
                    expected = string.Format(string.Format("{{0,{0}}}", width), o);
                    Assert.Equal(expected, actual);
                    sm.Dispose();
                }
            }

#if PUBLIC_STRINGMAKER
            sm = new StringMaker(Array.Empty<char>(), true);
            sm.Append(o);
            Assert.True(sm.Overflowed);
            Assert.Equal(string.Empty, sm.ExtractString());
            sm.Dispose();
#endif

            sm = new StringMaker(Array.Empty<char>(), true);
            sm.Append(o, 0);
            Assert.True(sm.Overflowed);
            Assert.Equal(string.Empty, sm.ExtractSpan().ToString());
            Assert.Equal(string.Empty, sm.ExtractString());
            sm.Dispose();
        }

        [Fact]
        public void TestExpansion_Span()
        {
            StringMaker sm;
            string expected;
            string actual;

            var o = "123456".AsSpan();
            for (int capacity = 0; capacity < 20; capacity++)
            {
#if PUBLIC_STRINGMAKER
                sm = new StringMaker(capacity);
                sm.Append(o);
                actual = sm.ExtractString();
                expected = string.Format("{0}", "123456");
                Assert.Equal(expected, actual);
                sm.Dispose();
#endif

                for (int width = -10; width < 10; width++)
                {
                    sm = new StringMaker(capacity);
                    sm.Append(o, width);
                    actual = sm.ExtractString();
                    expected = string.Format(string.Format("{{0,{0}}}", width), "123456");
                    Assert.Equal(expected, actual);
                    sm.Dispose();
                }
            }

#if PUBLIC_STRINGMAKER
            sm = new StringMaker(Array.Empty<char>(), true);
            sm.Append(o);
            Assert.True(sm.Overflowed);
            Assert.Equal(string.Empty, sm.ExtractString());
            sm.Dispose();
#endif

            sm = new StringMaker(Array.Empty<char>(), true);
            sm.Append(o, 0);
            Assert.True(sm.Overflowed);
            Assert.Equal(string.Empty, sm.ExtractSpan().ToString());
            Assert.Equal(string.Empty, sm.ExtractString());
            sm.Dispose();
        }

        [Fact]
        public void TestExpansion_Object()
        {
            StringMaker sm;
            string expected;
            string actual;

            var o = (object)"123456";
            for (int capacity = 0; capacity < 20; capacity++)
            {
#if PUBLIC_STRINGMAKER
                sm = new StringMaker(capacity);
                sm.Append(o);
                actual = sm.ExtractString();
                expected = string.Format("{0}", o);
                Assert.Equal(expected, actual);
                sm.Dispose();
#endif

                for (int width = -10; width < 10; width++)
                {
                    sm = new StringMaker(capacity);
                    sm.Append(o, width);
                    actual = sm.ExtractString();
                    expected = string.Format(string.Format("{{0,{0}}}", width), o);
                    Assert.Equal(expected, actual);
                    sm.Dispose();
                }
            }

#if PUBLIC_STRINGMAKER
            sm = new StringMaker(Array.Empty<char>(), true);
            sm.Append(o);
            Assert.True(sm.Overflowed);
            Assert.Equal(string.Empty, sm.ExtractString());
            sm.Dispose();
#endif

            sm = new StringMaker(Array.Empty<char>(), true);
            sm.Append(o, 0);
            Assert.True(sm.Overflowed);
            Assert.Equal(string.Empty, sm.ExtractSpan().ToString());
            Assert.Equal(string.Empty, sm.ExtractString());
            sm.Dispose();
        }

        private struct SpanFormattable : ISpanFormattable
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
            StringMaker sm;
            string expected;
            string actual;

            var o = default(SpanFormattable);
            for (int capacity = 0; capacity < 20; capacity++)
            {
#if PUBLIC_STRINGMAKER
                sm = new StringMaker(capacity);
                sm.Append(o);
                actual = sm.ExtractString();
                expected = string.Format("{0}", o);
                Assert.Equal(expected, actual);
                sm.Dispose();
#endif

                for (int width = -10; width < 10; width++)
                {
                    sm = new StringMaker(capacity);
                    sm.Append(o, string.Empty, null, width);
                    actual = sm.ExtractString();
                    expected = string.Format(string.Format("{{0,{0}}}", width), o);
                    Assert.Equal(expected, actual);
                    sm.Dispose();
                }
            }

#if PUBLIC_STRINGMAKER
            sm = new StringMaker(Array.Empty<char>(), true);
            sm.Append(o);
            Assert.True(sm.Overflowed);
            Assert.Equal(string.Empty, sm.ExtractString());
            sm.Dispose();
#endif

            sm = new StringMaker(Array.Empty<char>(), true);
            sm.Append(o, string.Empty, null, 0);
            Assert.True(sm.Overflowed);
            Assert.Equal(string.Empty, sm.ExtractSpan().ToString());
            Assert.Equal(string.Empty, sm.ExtractString());
            sm.Dispose();
        }

        private struct Formattable : IFormattable
        {
            public string ToString(string? format, IFormatProvider? formatProvider)
            {
                return "123456";
            }
        }

        [Fact]
        public void TestExpansion_Formattable()
        {
            StringMaker sm;
            string expected;
            string actual;

            Formattable o = default;
            for (int capacity = 0; capacity < 20; capacity++)
            {
#if PUBLIC_STRINGMAKER
                sm = new StringMaker(capacity);
                sm.Append(o);
                actual = sm.ExtractString();
                expected = string.Format("{0}", o);
                Assert.Equal(expected, actual);
                sm.Dispose();
#endif

                for (int width = -10; width < 10; width++)
                {
                    sm = new StringMaker(capacity);
                    sm.Append(o, string.Empty, null, width);
                    actual = sm.ExtractString();
                    expected = string.Format(string.Format("{{0,{0}}}", width), o);
                    Assert.Equal(expected, actual);
                    sm.Dispose();
                }
            }

#if PUBLIC_STRINGMAKER
            sm = new StringMaker(Array.Empty<char>(), true);
            sm.Append(o);
            Assert.True(sm.Overflowed);
            Assert.Equal(string.Empty, sm.ExtractString());
            sm.Dispose();
#endif

            sm = new StringMaker(Array.Empty<char>(), true);
            sm.Append(o, string.Empty, null, 0);
            Assert.True(sm.Overflowed);
            Assert.Equal(string.Empty, sm.ExtractSpan().ToString());
            Assert.Equal(string.Empty, sm.ExtractString());
            sm.Dispose();
        }

        [Fact]
        public void TestExpansion_Fill()
        {
            StringMaker sm;

            for (int capacity = 0; capacity < 20; capacity++)
            {
                sm = new StringMaker(capacity);
                sm.Fill('X', 6);
                var actual = sm.ExtractString();
                var expected = string.Format("{0}", "XXXXXX");
                Assert.Equal(expected, actual);
                sm.Dispose();
            }

            sm = new StringMaker(Array.Empty<char>(), true);
            sm.Fill('X', 6);
            Assert.True(sm.Overflowed);
            Assert.Equal(string.Empty, sm.ExtractString());
            sm.Dispose();

            sm = new StringMaker(Array.Empty<char>(), true);
            sm.Fill('X', 6);
            Assert.Equal(string.Empty, sm.ExtractSpan().ToString());
            sm.Dispose();

            sm = new StringMaker(Array.Empty<char>(), true);
            sm.Fill('X', 6);
            Assert.True(sm.Overflowed);
            Assert.Equal(string.Empty, sm.ExtractString());
            sm.Dispose();
        }

        [Fact]
        public void TestNullArgs()
        {
            StringMaker sm;

#if PUBLIC_STRINGMAKER
            sm = default;
            sm.Append((string?)null);
            Assert.Equal(string.Empty, sm.ExtractString());
            sm.Dispose();
#endif
            sm = default;
            sm.Append(null, 12);
            Assert.Equal("            ", sm.ExtractString());
            sm.Dispose();

#if PUBLIC_STRINGMAKER
            sm = default;
            sm.Append((object?)null);
            Assert.Equal(string.Empty, sm.ExtractString());
            sm.Dispose();
#endif

            sm = default;
            sm.Append((object?)null, 12);
            Assert.Equal("            ", sm.ExtractString());
            sm.Dispose();
        }

        [Fact]
        public void TestCapacity()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new StringMaker(-1));

            var sm = new StringMaker(0);
            Assert.Equal(0, sm.Length);
            Assert.False(sm.Overflowed);
            sm.Dispose();
        }
    }
}
