// © Microsoft Corporation. All rights reserved.

using System;
using System.Globalization;
using System.Text;
using Xunit;

#pragma warning disable S4056 // Overloads with a "CultureInfo" or an "IFormatProvider" parameter should be used
#pragma warning disable SA1011 // Closing square brackets should be spaced correctly

#if !STATIC_FORMAT
#pragma warning disable CA1801 // Review unused parameters
#endif

namespace Text.Formatting.Test
{
    public class FormatterTests
    {
        private static void CheckExpansion<T>(T arg)
        {
            var format = "{0,256} {1}";

            var expectedResult = string.Format(format, 3.14, arg);
            var sf = new StringFormatter(format);
            var actualResult1 = sf.Format(null, 3.14, arg);
            var actualResult2 = sf.Format(3.14, arg);

            Assert.Equal(expectedResult, actualResult1);
            Assert.Equal(expectedResult, actualResult2);
        }

        private static void CheckFormatWithString<T>(string? expectedResult, string format, T arg)
        {
            var sf = new StringFormatter(format);
            var actualResult1 = sf.Format(null, arg);
            var actualResult2 = sf.Format(arg);

            Assert.Equal(expectedResult, actualResult1);
            Assert.Equal(expectedResult, actualResult2);
        }

        private static void CheckFormatWithString<T0, T1>(string? expectedResult, string format, T0 arg0, T1 arg1)
        {
            var sf = new StringFormatter(format);
            var actualResult1 = sf.Format(null, arg0, arg1);
            var actualResult2 = sf.Format(arg0, arg1);

            Assert.Equal(expectedResult, actualResult1);
            Assert.Equal(expectedResult, actualResult2);
        }

        private static void CheckFormatWithString<T0, T1, T2>(string? expectedResult, string format, T0 arg0, T1 arg1, T2 arg2)
        {
            var sf = new StringFormatter(format);
            var actualResult1 = sf.Format(null, arg0, arg1, arg2);
            var actualResult2 = sf.Format(arg0, arg1, arg2);

            Assert.Equal(expectedResult, actualResult1);
            Assert.Equal(expectedResult, actualResult2);
        }

        private static void CheckFormatWithString<T0, T1, T2>(string? expectedResult, string format, T0 arg0, T1 arg1, T2 arg2, params object?[]? args)
        {
            var sf = new StringFormatter(format);
            var actualResult1 = sf.Format(null, arg0, arg1, arg2, args);
            var actualResult2 = sf.Format(arg0, arg1, arg2, args);

            Assert.Equal(expectedResult, actualResult1);
            Assert.Equal(expectedResult, actualResult2);
        }

        private static void CheckFormatWithString(string? expectedResult, string format, params object?[]? args)
        {
            var sf = new StringFormatter(format);
            var actualResult1 = sf.Format(null, args);
            var actualResult2 = sf.Format(args);

            Assert.Equal(expectedResult, actualResult1);
            Assert.Equal(expectedResult, actualResult2);
        }

        private static void CheckFormatWithSpan<T>(string? expectedResult, string format, T arg)
        {
            var sf = new StringFormatter(format);
            var s = new Span<char>(new char[(format.Length * 2) + 128]);
            Assert.True(sf.TryFormat(s, out int charsWritten, null, arg));
            var actualResult = s.Slice(0, charsWritten).ToString();

            Assert.Equal(expectedResult, actualResult);
        }

        private static void CheckFormatWithSpan<T0, T1>(string? expectedResult, string format, T0 arg0, T1 arg1)
        {
            var sf = new StringFormatter(format);
            var s = new Span<char>(new char[(format.Length * 2) + 128]);
            Assert.True(sf.TryFormat(s, out int charsWritten, null, arg0, arg1));
            var actualResult = s.Slice(0, charsWritten).ToString();

            Assert.Equal(expectedResult, actualResult);
        }

        private static void CheckFormatWithSpan<T0, T1, T2>(string? expectedResult, string format, T0 arg0, T1 arg1, T2 arg2)
        {
            var sf = new StringFormatter(format);
            var s = new Span<char>(new char[(format.Length * 2) + 128]);
            Assert.True(sf.TryFormat(s, out int charsWritten, null, arg0, arg1, arg2));
            var actualResult = s.Slice(0, charsWritten).ToString();

            Assert.Equal(expectedResult, actualResult);
        }

        private static void CheckFormatWithSpan<T0, T1, T2>(string? expectedResult, string format, T0 arg0, T1 arg1, T2 arg2, params object?[]? args)
        {
            var sf = new StringFormatter(format);
            var s = new Span<char>(new char[(format.Length * 2) + 128]);
            Assert.True(sf.TryFormat(s, out int charsWritten, null, arg0, arg1, arg2, args));
            var actualResult = s.Slice(0, charsWritten).ToString();

            Assert.Equal(expectedResult, actualResult);
        }

        private static void CheckFormatWithSpan(string? expectedResult, string format, params object?[]? args)
        {
            var sf = new StringFormatter(format);
            var s = new Span<char>(new char[(format.Length * 2) + 128]);
            Assert.True(sf.TryFormat(s, out int charsWritten, null, args));
            var actualResult = s.Slice(0, charsWritten).ToString();

            Assert.Equal(expectedResult, actualResult);
        }

        private static void CheckFormatWithStatic<T>(string? expectedResult, string format, T arg)
        {
            // optional feature
#if STATIC_FORMAT
            var actualResult1 = StringFormatter.Format(CultureInfo.CurrentCulture, format, arg);
            var actualResult2 = StringFormatter.Format(format, arg);

            Assert.Equal(expectedResult, actualResult1);
            Assert.Equal(expectedResult, actualResult2);
#endif
        }

        private static void CheckFormatWithStatic<T0, T1>(string? expectedResult, string format, T0 arg0, T1 arg1)
        {
            // optional feature
#if STATIC_FORMAT
            var actualResult1 = StringFormatter.Format(CultureInfo.CurrentCulture, format, arg0, arg1);
            var actualResult2 = StringFormatter.Format(format, arg0, arg1);

            Assert.Equal(expectedResult, actualResult1);
            Assert.Equal(expectedResult, actualResult2);
#endif
        }

        private static void CheckFormatWithStatic<T0, T1, T2>(string expectedResult, string format, T0 arg0, T1 arg1, T2 arg2)
        {
            // optional feature
#if STATIC_FORMAT
            var actualResult1 = StringFormatter.Format(CultureInfo.CurrentCulture, format, arg0, arg1, arg2);
            var actualResult2 = StringFormatter.Format(format, arg0, arg1, arg2);

            Assert.Equal(expectedResult, actualResult1);
            Assert.Equal(expectedResult, actualResult2);
#endif
        }

        private static void CheckFormatWithStatic<T0, T1, T2>(string? expectedResult, string format, T0 arg0, T1 arg1, T2 arg2, params object?[]? args)
        {
            // optional feature
#if STATIC_FORMAT
            var actualResult1 = StringFormatter.Format(CultureInfo.CurrentCulture, format, arg0, arg1, arg2, args);
            var actualResult2 = StringFormatter.Format(format, arg0, arg1, arg2, args);

            Assert.Equal(expectedResult, actualResult1);
            Assert.Equal(expectedResult, actualResult2);
#endif
        }

        private static void CheckFormatWithStatic(string? expectedResult, string format, params object?[]? args)
        {
            // optional feature
#if STATIC_FORMAT
            var actualResult1 = StringFormatter.Format(CultureInfo.CurrentCulture, format, args);
            var actualResult2 = StringFormatter.Format(format, args);

            Assert.Equal(expectedResult, actualResult1);
            Assert.Equal(expectedResult, actualResult2);
#endif
        }

        private static void CheckFormat<T>(string format, T arg)
        {
            var expectedResult = string.Format(format, arg);
            CheckFormatWithString(expectedResult, format, arg);
            CheckFormatWithSpan(expectedResult, format, arg);
            CheckFormatWithStatic(expectedResult, format, arg);
        }

        private static void CheckFormat<T0, T1>(string format, T0 arg0, T1 arg1)
        {
            var expectedResult = string.Format(format, arg0, arg1);
            CheckFormatWithString(expectedResult, format, arg0, arg1);
            CheckFormatWithSpan(expectedResult, format, arg0, arg1);
            CheckFormatWithStatic(expectedResult, format, arg0, arg1);
        }

        private static void CheckFormat<T0, T1, T2>(string format, T0 arg0, T1 arg1, T2 arg2)
        {
            var expectedResult = string.Format(format, arg0, arg1, arg2);
            CheckFormatWithString(expectedResult, format, arg0, arg1, arg2);
            CheckFormatWithSpan(expectedResult, format, arg0, arg1, arg2);
            CheckFormatWithStatic(expectedResult, format, arg0, arg1, arg2);
        }

        private static void CheckFormat<T0, T1, T2>(string format, T0 arg0, T1 arg1, T2 arg2, params object?[]? args)
        {
            int argLen = 3 + args!.Length;
            var a = new object?[argLen];
            a[0] = arg0;
            a[1] = arg1;
            a[2] = arg2;
            for (int i = 3; i < a.Length; i++)
            {
                a[i] = args![i - 3];
            }

            var expectedResult = string.Format(format, a);
            CheckFormatWithString(expectedResult, format, arg0, arg1, arg2, args);
            CheckFormatWithSpan(expectedResult, format, arg0, arg1, arg2, args);
            CheckFormatWithStatic(expectedResult, format, arg0, arg1, arg2, args);
        }

        private static void CheckFormat(string format, params object?[] args)
        {
            var expectedResult = string.Format(format, args);
            CheckFormatWithString(expectedResult, format, args);
            CheckFormatWithSpan(expectedResult, format, args);
            CheckFormatWithStatic(expectedResult, format, args);
        }

        [Theory]
        [InlineData("")]
        [InlineData("X")]
        [InlineData("XX")]
        public void NoArgs(string format)
        {
            CheckFormat(format);
        }

        [Fact]
        public void NoArgsLarge()
        {
            CheckFormat(new StringBuilder().Append('X', 32767).ToString());
            CheckFormat(new StringBuilder().Append('X', 32768).ToString());
            CheckFormat(new StringBuilder().Append('X', 65535).ToString());
            CheckFormat(new StringBuilder().Append('X', 65536).ToString());
        }

        [Theory]
        [InlineData("{0}", 42)]
        [InlineData("X{0}", 42)]
        [InlineData("{0}Y", 42)]
        [InlineData("X{0}Y", 42)]
        [InlineData("XZ{0}ZY", 42)]
        public void OneArg(string format, int arg)
        {
            CheckFormat(format, arg);
        }

        [Fact]
        public void OneArgLarge()
        {
            CheckFormat(new StringBuilder().Append('X', 65535) + "{0}", 42);
            CheckFormat("{0}" + new StringBuilder().Append('X', 65535), 42);
            CheckFormat(new StringBuilder().Append('X', 65535) + "{0}" + new StringBuilder().Append('X', 65535), 42);
        }

        [Theory]
        [InlineData("{0} {1}", 42, 3.14)]
        [InlineData("X{0}{1}", 42, 3.14)]
        [InlineData("{0} {1}Y", 42, 3.14)]
        [InlineData("X{0}{1}Y", 42, 3.14)]
        [InlineData("XZ{0} {1}ZY", 42, 3.14)]
        [InlineData("{0} {1} {0}", 42, 3.14)]
        [InlineData("X{0}{1} {0}", 42, 3.14)]
        [InlineData("{0} {1}Y {0}", 42, 3.14)]
        [InlineData("X{0}{1}Y {0}", 42, 3.14)]
        [InlineData("XZ{0} {1}ZY {0}", 42, 3.14)]
        public void TwoArgs(string format, int arg0, double arg1)
        {
            CheckFormat(format, arg0, arg1);
        }

        [Theory]
        [InlineData("{0} {1} {2}", 42, 3.14, "XX")]
        [InlineData("X{0}{1}{2}", 42, 3.14, "XX")]
        [InlineData("{0} {1} {2}Y", 42, 3.14, "XX")]
        [InlineData("X{0}{1}{2}Y", 42, 3.14, "XX")]
        [InlineData("XZ{0} {1} {2}ZY", 42, 3.14, "XX")]
        public void ThreeArgs(string format, int arg0, double arg1, string arg2)
        {
            CheckFormat(format, arg0, arg1, arg2);
        }

        [Theory]
        [InlineData("{0} {1} {2} {3}", 42, 3.14, "XX", true)]
        [InlineData("X{0}{1}{2}{3}", 42, 3.14, "XX", true)]
        [InlineData("{0} {1} {2} {3}Y", 42, 3.14, "XX", false)]
        [InlineData("X{0}{1}{2}{3}Y", 42, 3.14, "XX", true)]
        [InlineData("XZ{0} {1} {2} {3}ZY", 42, 3.14, "XX", false)]
        public void FourArgs(string format, int arg0, double arg1, string arg2, bool arg3)
        {
            CheckFormat(format, arg0, arg1, arg2, arg3);
        }

        [Fact]
        public void ArgArray()
        {
            CheckFormat("", Array.Empty<object>());
            CheckFormat("X", Array.Empty<object>());
            CheckFormat("XY", Array.Empty<object>());

            CheckFormat("{0}", new object[] { 42 });
            CheckFormat("X{0}", new object[] { 42 });
            CheckFormat("{0}Y", new object[] { 42 });
            CheckFormat("X{0}Y", new object[] { 42 });
            CheckFormat("XZ{0}ZY", new object[] { 42 });

            CheckFormat("{0} {1}", new object[] { 42, 3.14 });
            CheckFormat("X{0}{1}", new object[] { 42, 3.14 });
            CheckFormat("{0} {1}Y", new object[] { 42, 3.14 });
            CheckFormat("X{0}{1}Y", new object[] { 42, 3.14 });
            CheckFormat("XZ{0} {1}ZY", new object[] { 42, 3.14 });

            CheckFormat("{0} {1} {2}", new object[] { 42, 3.14, "XX" });
            CheckFormat("X{0}{1}{2}", new object[] { 42, 3.14, "XX" });
            CheckFormat("{0} {1} {2}Y", new object[] { 42, 3.14, "XX" });
            CheckFormat("X{0}{1}{2}Y", new object[] { 42, 3.14, "XX" });
            CheckFormat("XZ{0} {1} {2}ZY", new object[] { 42, 3.14, "XX" });

            CheckFormat("{0} {1} {2} {3}", new object[] { 42, 3.14, "XX", true });
            CheckFormat("X{0}{1}{2}{3}", new object[] { 42, 3.14, "XX", true });
            CheckFormat("{0} {1} {2} {3}Y", new object[] { 42, 3.14, "XX", false });
            CheckFormat("X{0}{1}{2}{3}Y", new object[] { 42, 3.14, "XX", true });
            CheckFormat("XZ{0} {1} {2} {3}ZY", new object[] { 42, 3.14, "XX", false });

            CheckFormat("XZ{0} {1} {2} {3}ZY", new object[] { "42", "3.14", "XX", "false" });
        }

        [Theory]
        [InlineData("{")]
        [InlineData("X{")]
        [InlineData("}")]
        [InlineData("X}")]
        [InlineData("{X}")]
        [InlineData("{0,/}")]
        [InlineData("{0,:}")]
        [InlineData("{100000000000000000000,2}")]
        [InlineData("{0")]
        [InlineData("{0,")]
        [InlineData("{0,}")]
        [InlineData("{0,-")]
        [InlineData("{0,-}")]
        [InlineData("{0,0")]
        [InlineData("{0,0X")]
        [InlineData("{0,1000000000000000000}")]
        [InlineData("{0,0:")]
        [InlineData("{0,0:{")]
        [InlineData("{ 0,0}")]
        [InlineData("{0,0:{{")]
        [InlineData("{0,0:}}")]
        [InlineData("{0,0:{{X}}")]
        [InlineData("{0  ")]
        [InlineData("{0,  ")]
        [InlineData("{0  X")]
        [InlineData("{0,  {")]
        public void BadFormatString(string format)
        {
            Assert.Throws<FormatException>(() => _ = string.Format(format, 1, 2, 3, 4, 5, 6, 7));
            Assert.Throws<ArgumentException>(() => _ = new StringFormatter(format));
        }

        [Theory]
        [InlineData("{0, 0}", 42)]
        [InlineData("{0 ,0}", 42)]
        [InlineData("{0 }", 42)]
        [InlineData("{0,0 }", 42)]
        [InlineData("{0,0 :x}", 42)]
        [InlineData("{0,0: X}", 42)]
        [InlineData("{0,0:X }", 42)]
        public void CheckWhitespace(string format, int arg)
        {
            CheckFormat(format, arg);
        }

        [Fact]
        public void CheckWidth()
        {
            for (int width = -10; width < 10; width++)
            {
                CheckFormat($"{{0,{width}}}", "X");
                CheckFormat($"{{0,{width}}}", "XY");
                CheckFormat($"{{0,{width}}}", "XYZ");
            }
        }

        [Theory]
        [InlineData("{{{0}", 42)]
        [InlineData("{{{0}}}", 42)]
        public void CheckEscapes(string format, int arg)
        {
            CheckFormat(format, arg);
        }

        [Fact]
        public void BadNumArgs()
        {
            var sf = new StringFormatter("{0} {2}");

            Assert.Throws<ArgumentException>(() => sf.Format((object?[])null!));
            Assert.Throws<ArgumentException>(() => sf.Format(null, 1, 2, 3, 4));
        }

        private struct Custom1 : IFormattable
        {
            public string ToString(string? format, IFormatProvider? formatProvider)
            {
                return "IFormattable Output";
            }
        }

        private struct Custom2 : ISpanFormattable, IFormattable
        {
            public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
            {
                if (destination.Length < 16)
                {
                    charsWritten = 0;
                    return false;
                }

                "TryFormat Output".AsSpan().CopyTo(destination);
                charsWritten = 16;
                return true;
            }

            // NOTE: If/when this test is built as part of the .NET release,
            //       this should be removed. It's needed because String.Format
            //       doesn't recognize my hacky ISpanFormattable.
            public string ToString(string? format, IFormatProvider? formatProvider)
            {
                return "TryFormat Output";
            }
        }

        [Fact]
        public void ArgTypes()
        {
            CheckFormat("{0}", (sbyte)42);
            CheckFormat("{0}", (short)42);
            CheckFormat("{0}", 42);
            CheckFormat("{0}", 42L);
            CheckFormat("{0}", (byte)42);
            CheckFormat("{0}", (ushort)42);
            CheckFormat("{0}", 42U);
            CheckFormat("{0}", 42UL);
            CheckFormat("{0}", 42.0F);
            CheckFormat("{0}", 42.0);
            CheckFormat("{0}", 'x');
            CheckFormat("{0}", DateTime.UtcNow);
            CheckFormat("{0}", new TimeSpan(42));
            CheckFormat("{0}", true);
            CheckFormat("{0}", new decimal(42.0));
            CheckFormat("{0}", new Guid(new byte[] { 42, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }));
            CheckFormat("{0}", "XYZ");
            CheckFormat("{0}", new object?[] { null });
            CheckFormat("{0}", default(Custom1));
            CheckFormat("{0}", default(Custom2));
        }

        [Fact]
        public void BufferExpansion()
        {
            CheckExpansion((sbyte)42);
            CheckExpansion((short)42);
            CheckExpansion(42);
            CheckExpansion(42L);
            CheckExpansion((byte)42);
            CheckExpansion((ushort)42);
            CheckExpansion(42U);
            CheckExpansion(42UL);
            CheckExpansion(42.0F);
            CheckExpansion(42.0);
            CheckExpansion('X');
            CheckExpansion(DateTime.UtcNow);
            CheckExpansion(new TimeSpan(42));
            CheckExpansion(true);
            CheckExpansion(new decimal(42.0));
            CheckExpansion(new Guid(new byte[] { 42, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }));
            CheckExpansion("XYZ");
            CheckExpansion(new object?[] { null });
            CheckExpansion(default(Custom1));
            CheckExpansion(default(Custom2));
        }

        [Theory]
        [InlineData("{0:d}", 0)]
        [InlineData("{0:d}", 5)]
        [InlineData("{0:d}", 10)]
        [InlineData("{0:d}", 15)]
        [InlineData("{0:d}", 100)]
        [InlineData("{0:d}", 123)]
        [InlineData("{0:d}", 1024)]
        [InlineData("{0:d}", -5)]
        [InlineData("{0:d}", -10)]
        [InlineData("{0:d}", -15)]
        [InlineData("{0:d}", -100)]
        [InlineData("{0:d}", -123)]
        [InlineData("{0:d}", -1024)]
        [InlineData("{0:d1}", 0)]
        [InlineData("{0:d1}", 5)]
        [InlineData("{0:d1}", 10)]
        [InlineData("{0:d1}", 15)]
        [InlineData("{0:d1}", 100)]
        [InlineData("{0:d1}", 123)]
        [InlineData("{0:d1}", 1024)]
        [InlineData("{0:d1}", -5)]
        [InlineData("{0:d1}", -10)]
        [InlineData("{0:d1}", -15)]
        [InlineData("{0:d1}", -100)]
        [InlineData("{0:d1}", -123)]
        [InlineData("{0:d1}", -1024)]
        [InlineData("{0:d2}", 0)]
        [InlineData("{0:d2}", 5)]
        [InlineData("{0:d2}", 10)]
        [InlineData("{0:d2}", 15)]
        [InlineData("{0:d2}", 100)]
        [InlineData("{0:d2}", 123)]
        [InlineData("{0:d2}", 1024)]
        [InlineData("{0:d2}", -5)]
        [InlineData("{0:d2}", -10)]
        [InlineData("{0:d2}", -15)]
        [InlineData("{0:d2}", -100)]
        [InlineData("{0:d2}", -123)]
        [InlineData("{0:d2}", -1024)]
        [InlineData("{0:d3}", 0)]
        [InlineData("{0:d3}", 5)]
        [InlineData("{0:d3}", 10)]
        [InlineData("{0:d3}", 15)]
        [InlineData("{0:d3}", 100)]
        [InlineData("{0:d3}", 123)]
        [InlineData("{0:d3}", 1024)]
        [InlineData("{0:d3}", -5)]
        [InlineData("{0:d3}", -10)]
        [InlineData("{0:d3}", -15)]
        [InlineData("{0:d3}", -100)]
        [InlineData("{0:d3}", -123)]
        [InlineData("{0:d3}", -1024)]
        [InlineData("{0:d4}", 0)]
        [InlineData("{0:d4}", 5)]
        [InlineData("{0:d4}", 10)]
        [InlineData("{0:d4}", 15)]
        [InlineData("{0:d4}", 100)]
        [InlineData("{0:d4}", 123)]
        [InlineData("{0:d4}", 1024)]
        [InlineData("{0:d4}", -5)]
        [InlineData("{0:d4}", -10)]
        [InlineData("{0:d4}", -15)]
        [InlineData("{0:d4}", -100)]
        [InlineData("{0:d4}", -123)]
        [InlineData("{0:d4}", -1024)]
        public void TestStringFormatD(string format, int arg)
        {
            CheckFormat(format, arg);
        }

        [Theory]
        [InlineData("{0,1:d}", 0)]
        [InlineData("{0,1:d}", 5)]
        [InlineData("{0,1:d}", 10)]
        [InlineData("{0,1:d}", 15)]
        [InlineData("{0,1:d}", 100)]
        [InlineData("{0,1:d}", 123)]
        [InlineData("{0,1:d}", 1024)]
        [InlineData("{0,1:d}", -5)]
        [InlineData("{0,1:d}", -10)]
        [InlineData("{0,1:d}", -15)]
        [InlineData("{0,1:d}", -100)]
        [InlineData("{0,1:d}", -123)]
        [InlineData("{0,1:d}", -1024)]
        [InlineData("{0,1:d1}", 0)]
        [InlineData("{0,1:d1}", 5)]
        [InlineData("{0,1:d1}", 10)]
        [InlineData("{0,1:d1}", 15)]
        [InlineData("{0,1:d1}", 100)]
        [InlineData("{0,1:d1}", 123)]
        [InlineData("{0,1:d1}", 1024)]
        [InlineData("{0,1:d1}", -5)]
        [InlineData("{0,1:d1}", -10)]
        [InlineData("{0,1:d1}", -15)]
        [InlineData("{0,1:d1}", -100)]
        [InlineData("{0,1:d1}", -123)]
        [InlineData("{0,1:d1}", -1024)]
        [InlineData("{0,1:d2}", 0)]
        [InlineData("{0,1:d2}", 5)]
        [InlineData("{0,1:d2}", 10)]
        [InlineData("{0,1:d2}", 15)]
        [InlineData("{0,1:d2}", 100)]
        [InlineData("{0,1:d2}", 123)]
        [InlineData("{0,1:d2}", 1024)]
        [InlineData("{0,1:d2}", -5)]
        [InlineData("{0,1:d2}", -10)]
        [InlineData("{0,1:d2}", -15)]
        [InlineData("{0,1:d2}", -100)]
        [InlineData("{0,1:d2}", -123)]
        [InlineData("{0,1:d2}", -1024)]
        [InlineData("{0,1:d3}", 0)]
        [InlineData("{0,1:d3}", 5)]
        [InlineData("{0,1:d3}", 10)]
        [InlineData("{0,1:d3}", 15)]
        [InlineData("{0,1:d3}", 100)]
        [InlineData("{0,1:d3}", 123)]
        [InlineData("{0,1:d3}", 1024)]
        [InlineData("{0,1:d3}", -5)]
        [InlineData("{0,1:d3}", -10)]
        [InlineData("{0,1:d3}", -15)]
        [InlineData("{0,1:d3}", -100)]
        [InlineData("{0,1:d3}", -123)]
        [InlineData("{0,1:d3}", -1024)]
        [InlineData("{0,1:d4}", 0)]
        [InlineData("{0,1:d4}", 5)]
        [InlineData("{0,1:d4}", 10)]
        [InlineData("{0,1:d4}", 15)]
        [InlineData("{0,1:d4}", 100)]
        [InlineData("{0,1:d4}", 123)]
        [InlineData("{0,1:d4}", 1024)]
        [InlineData("{0,1:d4}", -5)]
        [InlineData("{0,1:d4}", -10)]
        [InlineData("{0,1:d4}", -15)]
        [InlineData("{0,1:d4}", -100)]
        [InlineData("{0,1:d4}", -123)]
        [InlineData("{0,1:d4}", -1024)]
        public void TestStringFormatD1(string format, int arg)
        {
            CheckFormat(format, arg);
        }

        [Theory]
        [InlineData("{0,2:d}", 0)]
        [InlineData("{0,2:d}", 5)]
        [InlineData("{0,2:d}", 10)]
        [InlineData("{0,2:d}", 15)]
        [InlineData("{0,2:d}", 100)]
        [InlineData("{0,2:d}", 123)]
        [InlineData("{0,2:d}", 1024)]
        [InlineData("{0,2:d}", -5)]
        [InlineData("{0,2:d}", -10)]
        [InlineData("{0,2:d}", -15)]
        [InlineData("{0,2:d}", -100)]
        [InlineData("{0,2:d}", -123)]
        [InlineData("{0,2:d}", -1024)]
        [InlineData("{0,2:d1}", 0)]
        [InlineData("{0,2:d1}", 5)]
        [InlineData("{0,2:d1}", 10)]
        [InlineData("{0,2:d1}", 15)]
        [InlineData("{0,2:d1}", 100)]
        [InlineData("{0,2:d1}", 123)]
        [InlineData("{0,2:d1}", 1024)]
        [InlineData("{0,2:d1}", -5)]
        [InlineData("{0,2:d1}", -10)]
        [InlineData("{0,2:d1}", -15)]
        [InlineData("{0,2:d1}", -100)]
        [InlineData("{0,2:d1}", -123)]
        [InlineData("{0,2:d1}", -1024)]
        [InlineData("{0,2:d2}", 0)]
        [InlineData("{0,2:d2}", 5)]
        [InlineData("{0,2:d2}", 10)]
        [InlineData("{0,2:d2}", 15)]
        [InlineData("{0,2:d2}", 100)]
        [InlineData("{0,2:d2}", 123)]
        [InlineData("{0,2:d2}", 1024)]
        [InlineData("{0,2:d2}", -5)]
        [InlineData("{0,2:d2}", -10)]
        [InlineData("{0,2:d2}", -15)]
        [InlineData("{0,2:d2}", -100)]
        [InlineData("{0,2:d2}", -123)]
        [InlineData("{0,2:d2}", -1024)]
        [InlineData("{0,2:d3}", 0)]
        [InlineData("{0,2:d3}", 5)]
        [InlineData("{0,2:d3}", 10)]
        [InlineData("{0,2:d3}", 15)]
        [InlineData("{0,2:d3}", 100)]
        [InlineData("{0,2:d3}", 123)]
        [InlineData("{0,2:d3}", 1024)]
        [InlineData("{0,2:d3}", -5)]
        [InlineData("{0,2:d3}", -10)]
        [InlineData("{0,2:d3}", -15)]
        [InlineData("{0,2:d3}", -100)]
        [InlineData("{0,2:d3}", -123)]
        [InlineData("{0,2:d3}", -1024)]
        [InlineData("{0,2:d4}", 0)]
        [InlineData("{0,2:d4}", 5)]
        [InlineData("{0,2:d4}", 10)]
        [InlineData("{0,2:d4}", 15)]
        [InlineData("{0,2:d4}", 100)]
        [InlineData("{0,2:d4}", 123)]
        [InlineData("{0,2:d4}", 1024)]
        [InlineData("{0,2:d4}", -5)]
        [InlineData("{0,2:d4}", -10)]
        [InlineData("{0,2:d4}", -15)]
        [InlineData("{0,2:d4}", -100)]
        [InlineData("{0,2:d4}", -123)]
        [InlineData("{0,2:d4}", -1024)]
        public void TestStringFormatD2(string format, int arg)
        {
            CheckFormat(format, arg);
        }

        [Theory]
        [InlineData("{0,3:d}", 0)]
        [InlineData("{0,3:d}", 5)]
        [InlineData("{0,3:d}", 10)]
        [InlineData("{0,3:d}", 15)]
        [InlineData("{0,3:d}", 100)]
        [InlineData("{0,3:d}", 123)]
        [InlineData("{0,3:d}", 1024)]
        [InlineData("{0,3:d}", -5)]
        [InlineData("{0,3:d}", -10)]
        [InlineData("{0,3:d}", -15)]
        [InlineData("{0,3:d}", -100)]
        [InlineData("{0,3:d}", -123)]
        [InlineData("{0,3:d}", -1024)]
        [InlineData("{0,3:d1}", 0)]
        [InlineData("{0,3:d1}", 5)]
        [InlineData("{0,3:d1}", 10)]
        [InlineData("{0,3:d1}", 15)]
        [InlineData("{0,3:d1}", 100)]
        [InlineData("{0,3:d1}", 123)]
        [InlineData("{0,3:d1}", 1024)]
        [InlineData("{0,3:d1}", -5)]
        [InlineData("{0,3:d1}", -10)]
        [InlineData("{0,3:d1}", -15)]
        [InlineData("{0,3:d1}", -100)]
        [InlineData("{0,3:d1}", -123)]
        [InlineData("{0,3:d1}", -1024)]
        [InlineData("{0,3:d2}", 0)]
        [InlineData("{0,3:d2}", 5)]
        [InlineData("{0,3:d2}", 10)]
        [InlineData("{0,3:d2}", 15)]
        [InlineData("{0,3:d2}", 100)]
        [InlineData("{0,3:d2}", 123)]
        [InlineData("{0,3:d2}", 1024)]
        [InlineData("{0,3:d2}", -5)]
        [InlineData("{0,3:d2}", -10)]
        [InlineData("{0,3:d2}", -15)]
        [InlineData("{0,3:d2}", -100)]
        [InlineData("{0,3:d2}", -123)]
        [InlineData("{0,3:d2}", -1024)]
        [InlineData("{0,3:d3}", 0)]
        [InlineData("{0,3:d3}", 5)]
        [InlineData("{0,3:d3}", 10)]
        [InlineData("{0,3:d3}", 15)]
        [InlineData("{0,3:d3}", 100)]
        [InlineData("{0,3:d3}", 123)]
        [InlineData("{0,3:d3}", 1024)]
        [InlineData("{0,3:d3}", -5)]
        [InlineData("{0,3:d3}", -10)]
        [InlineData("{0,3:d3}", -15)]
        [InlineData("{0,3:d3}", -100)]
        [InlineData("{0,3:d3}", -123)]
        [InlineData("{0,3:d3}", -1024)]
        [InlineData("{0,3:d4}", 0)]
        [InlineData("{0,3:d4}", 5)]
        [InlineData("{0,3:d4}", 10)]
        [InlineData("{0,3:d4}", 15)]
        [InlineData("{0,3:d4}", 100)]
        [InlineData("{0,3:d4}", 123)]
        [InlineData("{0,3:d4}", 1024)]
        [InlineData("{0,3:d4}", -5)]
        [InlineData("{0,3:d4}", -10)]
        [InlineData("{0,3:d4}", -15)]
        [InlineData("{0,3:d4}", -100)]
        [InlineData("{0,3:d4}", -123)]
        [InlineData("{0,3:d4}", -1024)]
        public void TestStringFormatD3(string format, int arg)
        {
            CheckFormat(format, arg);
        }

        [Theory]
        [InlineData("{0,4:d}", 0)]
        [InlineData("{0,4:d}", 5)]
        [InlineData("{0,4:d}", 10)]
        [InlineData("{0,4:d}", 15)]
        [InlineData("{0,4:d}", 100)]
        [InlineData("{0,4:d}", 123)]
        [InlineData("{0,4:d}", 1024)]
        [InlineData("{0,4:d}", -5)]
        [InlineData("{0,4:d}", -10)]
        [InlineData("{0,4:d}", -15)]
        [InlineData("{0,4:d}", -100)]
        [InlineData("{0,4:d}", -123)]
        [InlineData("{0,4:d}", -1024)]
        [InlineData("{0,4:d1}", 0)]
        [InlineData("{0,4:d1}", 5)]
        [InlineData("{0,4:d1}", 10)]
        [InlineData("{0,4:d1}", 15)]
        [InlineData("{0,4:d1}", 100)]
        [InlineData("{0,4:d1}", 123)]
        [InlineData("{0,4:d1}", 1024)]
        [InlineData("{0,4:d1}", -5)]
        [InlineData("{0,4:d1}", -10)]
        [InlineData("{0,4:d1}", -15)]
        [InlineData("{0,4:d1}", -100)]
        [InlineData("{0,4:d1}", -123)]
        [InlineData("{0,4:d1}", -1024)]
        [InlineData("{0,4:d2}", 0)]
        [InlineData("{0,4:d2}", 5)]
        [InlineData("{0,4:d2}", 10)]
        [InlineData("{0,4:d2}", 15)]
        [InlineData("{0,4:d2}", 100)]
        [InlineData("{0,4:d2}", 123)]
        [InlineData("{0,4:d2}", 1024)]
        [InlineData("{0,4:d2}", -5)]
        [InlineData("{0,4:d2}", -10)]
        [InlineData("{0,4:d2}", -15)]
        [InlineData("{0,4:d2}", -100)]
        [InlineData("{0,4:d2}", -123)]
        [InlineData("{0,4:d2}", -1024)]
        [InlineData("{0,4:d3}", 0)]
        [InlineData("{0,4:d3}", 5)]
        [InlineData("{0,4:d3}", 10)]
        [InlineData("{0,4:d3}", 15)]
        [InlineData("{0,4:d3}", 100)]
        [InlineData("{0,4:d3}", 123)]
        [InlineData("{0,4:d3}", 1024)]
        [InlineData("{0,4:d3}", -5)]
        [InlineData("{0,4:d3}", -10)]
        [InlineData("{0,4:d3}", -15)]
        [InlineData("{0,4:d3}", -100)]
        [InlineData("{0,4:d3}", -123)]
        [InlineData("{0,4:d3}", -1024)]
        [InlineData("{0,4:d4}", 0)]
        [InlineData("{0,4:d4}", 5)]
        [InlineData("{0,4:d4}", 10)]
        [InlineData("{0,4:d4}", 15)]
        [InlineData("{0,4:d4}", 100)]
        [InlineData("{0,4:d4}", 123)]
        [InlineData("{0,4:d4}", 1024)]
        [InlineData("{0,4:d4}", -5)]
        [InlineData("{0,4:d4}", -10)]
        [InlineData("{0,4:d4}", -15)]
        [InlineData("{0,4:d4}", -100)]
        [InlineData("{0,4:d4}", -123)]
        [InlineData("{0,4:d4}", -1024)]
        public void TestStringFormatD4(string format, int arg)
        {
            CheckFormat(format, arg);
        }

        [Theory]
        [InlineData("{0}", 1)]
        [InlineData("{0}{1}", 2)]
        [InlineData("{0,3}", 1)]
        [InlineData("{0,3:d}", 1)]
        [InlineData("{0,3:d}{0}", 1)]
        [InlineData("{0,3:d}{1}", 2)]
        [InlineData("{0,3:d}{9}", 10)]
        public void TestNumArgsNeeded(string format, int argsExpected)
        {
            var sf = new StringFormatter(format);
            Assert.Equal(argsExpected, sf.NumArgumentsNeeded);
        }

        [Fact]
        public void OverflowNoArgs()
        {
            var sf = new StringFormatter("0123");
            Assert.False(sf.TryFormat(new char[3], out var charsWritten, null, null));
            Assert.Equal(0, charsWritten);

            Assert.True(sf.TryFormat(new char[4], out charsWritten, null, null));
            Assert.Equal(4, charsWritten);
        }

#if STATIC_FORMAT
        [Fact]
        public void LotsOfFormatting()
        {
            for (int i = 0; i < 256; i++)
            {
                var fmt = $"{{0,{i}}}";

                var expected = string.Format(fmt, i);
                var actual = StringFormatter.Format(fmt, i);

                Assert.Equal(expected, actual);
            }
        }
#endif
    }
}
