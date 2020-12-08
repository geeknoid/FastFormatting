// © Microsoft Corporation. All rights reserved.

namespace FastFormatting.Tests
{
    using System;
    using System.Globalization;
    using System.Text;
    using Xunit;
    using Xunit.Abstractions;

    public class FormatterTests
    {
        private readonly ITestOutputHelper _output;

        public FormatterTests(ITestOutputHelper output)
        {
            _output = output;
        }

        // do a formatting operation, comparing the results against classic String.Format
        private void CheckExpansion<T>(T arg)
        {
            string? expectedResult = null;
            string? actualResult1 = null;

            var format = "{0,256} {1}";

            expectedResult = String.Format(format, 3.14, arg);
            var sf = new StringFormatter(format);
            actualResult1 = sf.Format(null, 3.14, arg);
            Assert.Equal(expectedResult, actualResult1);
        }

        // do a formatting operation, comparing the results against classic String.Format
        private void CheckFormat<T>(string format, T arg)
        {
            string? expectedResult = null;
            string? actualResult1 = null;
            string? actualResult2 = null;
            string? actualResult3 = null;
            string? actualResult4 = null;

            expectedResult = String.Format(format, arg);

            var sf = new StringFormatter(format);
            actualResult1 = sf.Format(null, arg);

            var s = new Span<char>(new char[format.Length * 2 + 128]);
            if (sf.TryFormat(s, out int charsWritten, null, arg))
            {
                actualResult2 = s.Slice(0, charsWritten).ToString();
            }

            actualResult3 = StringFormatter.Format(CultureInfo.CurrentCulture, format, arg);
            actualResult4 = StringFormatter.Format(format, arg);

            Assert.Equal(expectedResult, actualResult1);
            Assert.Equal(expectedResult, actualResult2);
            Assert.Equal(expectedResult, actualResult3);
            Assert.Equal(expectedResult, actualResult4);
        }

        // do a formatting operation, comparing the results against classic String.Format
        private void CheckFormat<T0, T1>(string format, T0 arg0, T1 arg1)
        {
            string? expectedResult = null;
            string? actualResult1 = null;
            string? actualResult2 = null;
            string? actualResult3 = null;
            string? actualResult4 = null;

            expectedResult = String.Format(format, arg0, arg1);

            var sf = new StringFormatter(format);
            actualResult1 = sf.Format(null, arg0, arg1);

            sf = new StringFormatter(format);
            var s = new Span<char>(new char[format.Length * 2 + 128]);
            if (sf.TryFormat(s, out int charsWritten, null, arg0, arg1))
            {
                actualResult2 = s.Slice(0, charsWritten).ToString();
            }

            actualResult3 = StringFormatter.Format(CultureInfo.CurrentCulture, format, arg0, arg1);
            actualResult4 = StringFormatter.Format(format, arg0, arg1);

            Assert.Equal(expectedResult, actualResult1);
            Assert.Equal(expectedResult, actualResult2);
            Assert.Equal(expectedResult, actualResult3);
            Assert.Equal(expectedResult, actualResult4);
        }

        // do a formatting operation, comparing the results against classic String.Format
        private void CheckFormat<T0, T1, T2>(string format, T0 arg0, T1 arg1, T2 arg2)
        {
            string? expectedResult = null;
            string? actualResult1 = null;
            string? actualResult2 = null;
            string? actualResult3 = null;
            string? actualResult4 = null;

            expectedResult = String.Format(format, arg0, arg1, arg2);

            var sf = new StringFormatter(format);
            actualResult1 = sf.Format(null, arg0, arg1, arg2);

            sf = new StringFormatter(format);
            var s = new Span<char>(new char[format.Length * 2 + 128]);
            if (sf.TryFormat(s, out int charsWritten, null, arg0, arg1, arg2))
            {
                actualResult2 = s.Slice(0, charsWritten).ToString();
            }

            actualResult3 = StringFormatter.Format(CultureInfo.CurrentCulture, format, arg0, arg1, arg2);
            actualResult4 = StringFormatter.Format(format, arg0, arg1, arg2);

            Assert.Equal(expectedResult, actualResult1);
            Assert.Equal(expectedResult, actualResult2);
            Assert.Equal(expectedResult, actualResult3);
            Assert.Equal(expectedResult, actualResult4);
        }

        // do a formatting operation, comparing the results against classic String.Format
        private void CheckFormat<T0, T1, T2>(string format, T0 arg0, T1 arg1, T2 arg2, params object?[]? args)
        {
            string? expectedResult = null;
            string? actualResult1 = null;
            string? actualResult2 = null;
            string? actualResult3 = null;
            string? actualResult4 = null;

            int argLen = 3;
            if (args != null)
            {
                argLen += args.Length;
            }

            var a = new object?[argLen];
            a[0] = arg0;
            a[1] = arg1;
            a[2] = arg2;
            for (int i = 3; i < a.Length; i++)
            {
                a[i] = args![i - 3];
            }

            expectedResult = String.Format(format, a);

            var sf = new StringFormatter(format);
            actualResult1 = sf.Format(null, arg0, arg1, arg2, args);

            sf = new StringFormatter(format);
            var s = new Span<char>(new char[format.Length * 2 + 128]);
            if (sf.TryFormat(s, out int charsWritten, null, arg0, arg1, arg2, args))
            {
                actualResult2 = s.Slice(0, charsWritten).ToString();
            }

            actualResult3 = StringFormatter.Format(CultureInfo.CurrentCulture, format, arg0, arg1, arg2, args);
            actualResult4 = StringFormatter.Format(format, arg0, arg1, arg2, args);

            Assert.Equal(expectedResult, actualResult1);
            Assert.Equal(expectedResult, actualResult2);
            Assert.Equal(expectedResult, actualResult3);
            Assert.Equal(expectedResult, actualResult4);
        }

        // do a formatting operation, comparing the results against classic String.Format
        private void CheckFormat(string format, params object?[] args)
        {
            string? expectedResult = null;
            string? actualResult1 = null;
            string? actualResult2 = null;
            string? actualResult3 = null;
            string? actualResult4 = null;

            expectedResult = String.Format(format, args);

            var sf = new StringFormatter(format);
            actualResult1 = sf.Format(null, args);

            sf = new StringFormatter(format);
            var s = new Span<char>(new char[format.Length * 2]);
            if (sf.TryFormat(s, out int charsWritten, null, args))
            {
                actualResult2 = s.Slice(0, charsWritten).ToString();
            }

            actualResult3 = StringFormatter.Format(CultureInfo.CurrentCulture, format, args);
            actualResult4 = StringFormatter.Format(format, args);

            Assert.Equal(expectedResult, actualResult1);
            Assert.Equal(expectedResult, actualResult2);
            Assert.Equal(expectedResult, actualResult3);
            Assert.Equal(expectedResult, actualResult4);
        }

        private void CheckBadFormatString(string format)
        {
            Assert.Throws<FormatException>(() => _ = string.Format(format, 1, 2, 3, 4, 5, 6, 7));
            Assert.Throws<FormatException>(() => _ = new StringFormatter(format));
        }

        [Fact]
        public void NoArgs()
        {
            CheckFormat("");
            CheckFormat("X");
            CheckFormat("XX");
            CheckFormat(new StringBuilder().Append('X', 32767).ToString());
            CheckFormat(new StringBuilder().Append('X', 32768).ToString());
            CheckFormat(new StringBuilder().Append('X', 65535).ToString());
            CheckFormat(new StringBuilder().Append('X', 65536).ToString());
        }

        [Fact]
        public void OneArg()
        {
            CheckFormat("{0}", 42);
            CheckFormat("X{0}", 42);
            CheckFormat("{0}Y", 42);
            CheckFormat("X{0}Y", 42);
            CheckFormat("XZ{0}ZY", 42);
            CheckFormat(new StringBuilder().Append('X', 65535).ToString() + "{0}", 42);
            CheckFormat("{0}" + new StringBuilder().Append('X', 65535).ToString(), 42);
            CheckFormat(new StringBuilder().Append('X', 65535).ToString() + "{0}" + new StringBuilder().Append('X', 65535).ToString(), 42);
        }

        [Fact]
        public void TwoArgs()
        {
            CheckFormat("{0} {1}", 42, 3.14);
            CheckFormat("X{0}{1}", 42, 3.14);
            CheckFormat("{0} {1}Y", 42, 3.14);
            CheckFormat("X{0}{1}Y", 42, 3.14);
            CheckFormat("XZ{0} {1}ZY", 42, 3.14);

            CheckFormat("{0} {1} {0}", 42, 3.14);
            CheckFormat("X{0}{1} {0}", 42, 3.14);
            CheckFormat("{0} {1}Y {0}", 42, 3.14);
            CheckFormat("X{0}{1}Y {0}", 42, 3.14);
            CheckFormat("XZ{0} {1}ZY {0}", 42, 3.14);
        }

        [Fact]
        public void ThreeArgs()
        {
            CheckFormat("{0} {1} {2}", 42, 3.14, "XX");
            CheckFormat("X{0}{1}{2}", 42, 3.14, "XX");
            CheckFormat("{0} {1} {2}Y", 42, 3.14, "XX");
            CheckFormat("X{0}{1}{2}Y", 42, 3.14, "XX");
            CheckFormat("XZ{0} {1} {2}ZY", 42, 3.14, "XX");
        }

        [Fact]
        public void FourArgs()
        {
            CheckFormat("{0} {1} {2} {3}", 42, 3.14, "XX", true);
            CheckFormat("X{0}{1}{2}{3}", 42, 3.14, "XX", true);
            CheckFormat("{0} {1} {2} {3}Y", 42, 3.14, "XX", false);
            CheckFormat("X{0}{1}{2}{3}Y", 42, 3.14, "XX", true);
            CheckFormat("XZ{0} {1} {2} {3}ZY", 42, 3.14, "XX", false);
        }

        [Fact]
        public void ArgArray()
        {
            CheckFormat("", new object[] { });
            CheckFormat("X", new object[] { });
            CheckFormat("XY", new object[] { });

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

            CheckFormat("{0} {1} {2} {3}", new object[] {42, 3.14, "XX", true });
            CheckFormat("X{0}{1}{2}{3}", new object[] { 42, 3.14, "XX", true });
            CheckFormat("{0} {1} {2} {3}Y", new object[] { 42, 3.14, "XX", false });
            CheckFormat("X{0}{1}{2}{3}Y", new object[] { 42, 3.14, "XX", true });
            CheckFormat("XZ{0} {1} {2} {3}ZY", new object[] { 42, 3.14, "XX", false });

            CheckFormat("XZ{0} {1} {2} {3}ZY", new object[] { "42", "3.14", "XX", "false" });
        }

        [Fact]
        public void BadFormatString()
        {
            CheckBadFormatString("{");
            CheckBadFormatString("X{");
            CheckBadFormatString("{}");
            CheckBadFormatString("}");
            CheckBadFormatString("X}");
            CheckBadFormatString("{X}");
            CheckBadFormatString("{0,X}");
            CheckBadFormatString("{100000000000000000000,2}");
            CheckBadFormatString("{0");
            CheckBadFormatString("{0,");
            CheckBadFormatString("{0,}");
            CheckBadFormatString("{0,-");
            CheckBadFormatString("{0,-}");
            CheckBadFormatString("{0,0");
            CheckBadFormatString("{0,0X");
            CheckBadFormatString("{0,1000000000000000000}");
            CheckBadFormatString("{0,0:");
            CheckBadFormatString("{0,0:{");
            CheckBadFormatString("{ 0,0}");
            CheckBadFormatString("{0,0:{{");
            CheckBadFormatString("{0,0:}}");
            CheckBadFormatString("{0,0:{{X}}");
            CheckBadFormatString("{0,0:}}");

            Assert.Throws<ArgumentNullException>(() => _ = new StringFormatter(null!));
        }

        [Fact]
        public void CheckWhitespace()
        {
            CheckFormat("{0, 0}", 42);
            CheckFormat("{0 ,0}", 42);
            CheckFormat("{0 }", 42);
            CheckFormat("{0, 0}", 42);
            CheckFormat("{0,0 }", 42);
            CheckFormat("{0,0 :x}", 42);
            CheckFormat("{0,0: X}", 42);
            CheckFormat("{0,0:X }", 42);
        }

        [Fact]
        public void CheckWidth()
        {
            for (int width = -10; width < 10; width++)
            {
                _output.WriteLine($"Width {width}");

                CheckFormat($"{{0,{width}}}", "X");
                CheckFormat($"{{0,{width}}}", "XY");
                CheckFormat($"{{0,{width}}}", "XYZ");
            }
        }

        [Fact]
        public void CheckEscapes()
        {
            CheckFormat("{{{0}", 42);
            CheckFormat("{{{0}}}", 42);
        }

        [Fact]
        public void BadNumArgs()
        {
            var sf = new StringFormatter("{0} {2}");

            Assert.Throws<ArgumentException>(() => sf.Format(null));
            Assert.Throws<ArgumentException>(() => sf.Format(null, 1, 2, 3, 4));
        }

        struct Custom1 : IFormattable
        {
            public string ToString(string? format, IFormatProvider? formatProvider)
            {
                return "IFormattable Output";
            }
        }

        struct Custom2 : ISpanFormattable, IFormattable
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

            // NOTE: If/when this test is build as part of the .NET release,
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
            CheckFormat("{0}", (int)42);
            CheckFormat("{0}", (long)42);
            CheckFormat("{0}", (byte)42);
            CheckFormat("{0}", (ushort)42);
            CheckFormat("{0}", (uint)42);
            CheckFormat("{0}", (ulong)42);
            CheckFormat("{0}", (float)42.0);
            CheckFormat("{0}", (double)42.0);
            CheckFormat("{0}", 'x');
            CheckFormat("{0}", DateTime.Now);
            CheckFormat("{0}", new TimeSpan(42));
            CheckFormat("{0}", true);
            CheckFormat("{0}", new Decimal(42.0));
            CheckFormat("{0}", new Guid(new byte[] { 42, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }));
            CheckFormat("{0}", "XYZ");
            CheckFormat("{0}", new object?[] { null });
            CheckFormat("{0}", new Custom1());
            CheckFormat("{0}", new Custom2());
        }

        [Fact]
        public void BufferExpansion()
        {
            CheckExpansion((sbyte)42);
            CheckExpansion((short)42);
            CheckExpansion((int)42);
            CheckExpansion((long)42);
            CheckExpansion((byte)42);
            CheckExpansion((ushort)42);
            CheckExpansion((uint)42);
            CheckExpansion((ulong)42);
            CheckExpansion((float)42.0);
            CheckExpansion((double)42.0);
            CheckExpansion('X');
            CheckExpansion(DateTime.Now);
            CheckExpansion(new TimeSpan(42));
            CheckExpansion(true);
            CheckExpansion(new Decimal(42.0));
            CheckExpansion(new Guid(new byte[] { 42, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }));
            CheckExpansion("XYZ");
            CheckExpansion(new object?[] { null });
            CheckExpansion(new Custom1());
            CheckExpansion(new Custom2());
        }

        [Fact]
        public void TestStringFormatD()
        {
            CheckFormat("{0:d}", 0); 
            CheckFormat("{0:d}", 5); 
            CheckFormat("{0:d}", 10); 
            CheckFormat("{0:d}", 15); 
            CheckFormat("{0:d}", 100); 
            CheckFormat("{0:d}", 123); 
            CheckFormat("{0:d}", 1024); 
            CheckFormat("{0:d}", -5); 
            CheckFormat("{0:d}", -10); 
            CheckFormat("{0:d}", -15); 
            CheckFormat("{0:d}", -100); 
            CheckFormat("{0:d}", -123); 
            CheckFormat("{0:d}", -1024); 
            CheckFormat("{0:d1}", 0); 
            CheckFormat("{0:d1}", 5); 
            CheckFormat("{0:d1}", 10); 
            CheckFormat("{0:d1}", 15); 
            CheckFormat("{0:d1}", 100); 
            CheckFormat("{0:d1}", 123); 
            CheckFormat("{0:d1}", 1024); 
            CheckFormat("{0:d1}", -5); 
            CheckFormat("{0:d1}", -10); 
            CheckFormat("{0:d1}", -15); 
            CheckFormat("{0:d1}", -100); 
            CheckFormat("{0:d1}", -123); 
            CheckFormat("{0:d1}", -1024); 
            CheckFormat("{0:d2}", 0); 
            CheckFormat("{0:d2}", 5); 
            CheckFormat("{0:d2}", 10); 
            CheckFormat("{0:d2}", 15); 
            CheckFormat("{0:d2}", 100); 
            CheckFormat("{0:d2}", 123); 
            CheckFormat("{0:d2}", 1024); 
            CheckFormat("{0:d2}", -5); 
            CheckFormat("{0:d2}", -10); 
            CheckFormat("{0:d2}", -15); 
            CheckFormat("{0:d2}", -100); 
            CheckFormat("{0:d2}", -123); 
            CheckFormat("{0:d2}", -1024); 
            CheckFormat("{0:d3}", 0); 
            CheckFormat("{0:d3}", 5); 
            CheckFormat("{0:d3}", 10); 
            CheckFormat("{0:d3}", 15); 
            CheckFormat("{0:d3}", 100); 
            CheckFormat("{0:d3}", 123); 
            CheckFormat("{0:d3}", 1024); 
            CheckFormat("{0:d3}", -5); 
            CheckFormat("{0:d3}", -10); 
            CheckFormat("{0:d3}", -15); 
            CheckFormat("{0:d3}", -100); 
            CheckFormat("{0:d3}", -123); 
            CheckFormat("{0:d3}", -1024); 
            CheckFormat("{0:d4}", 0); 
            CheckFormat("{0:d4}", 5); 
            CheckFormat("{0:d4}", 10); 
            CheckFormat("{0:d4}", 15); 
            CheckFormat("{0:d4}", 100); 
            CheckFormat("{0:d4}", 123); 
            CheckFormat("{0:d4}", 1024); 
            CheckFormat("{0:d4}", -5); 
            CheckFormat("{0:d4}", -10); 
            CheckFormat("{0:d4}", -15); 
            CheckFormat("{0:d4}", -100); 
            CheckFormat("{0:d4}", -123); 
            CheckFormat("{0:d4}", -1024); 
        }

        [Fact]
        public void TestStringFormatD1()
        {
            CheckFormat("{0,1:d}", 0); 
            CheckFormat("{0,1:d}", 5); 
            CheckFormat("{0,1:d}", 10); 
            CheckFormat("{0,1:d}", 15); 
            CheckFormat("{0,1:d}", 100); 
            CheckFormat("{0,1:d}", 123); 
            CheckFormat("{0,1:d}", 1024); 
            CheckFormat("{0,1:d}", -5); 
            CheckFormat("{0,1:d}", -10); 
            CheckFormat("{0,1:d}", -15); 
            CheckFormat("{0,1:d}", -100); 
            CheckFormat("{0,1:d}", -123); 
            CheckFormat("{0,1:d}", -1024); 
            CheckFormat("{0,1:d1}", 0); 
            CheckFormat("{0,1:d1}", 5); 
            CheckFormat("{0,1:d1}", 10); 
            CheckFormat("{0,1:d1}", 15); 
            CheckFormat("{0,1:d1}", 100); 
            CheckFormat("{0,1:d1}", 123); 
            CheckFormat("{0,1:d1}", 1024); 
            CheckFormat("{0,1:d1}", -5); 
            CheckFormat("{0,1:d1}", -10); 
            CheckFormat("{0,1:d1}", -15); 
            CheckFormat("{0,1:d1}", -100); 
            CheckFormat("{0,1:d1}", -123); 
            CheckFormat("{0,1:d1}", -1024); 
            CheckFormat("{0,1:d2}", 0); 
            CheckFormat("{0,1:d2}", 5); 
            CheckFormat("{0,1:d2}", 10); 
            CheckFormat("{0,1:d2}", 15); 
            CheckFormat("{0,1:d2}", 100); 
            CheckFormat("{0,1:d2}", 123); 
            CheckFormat("{0,1:d2}", 1024); 
            CheckFormat("{0,1:d2}", -5); 
            CheckFormat("{0,1:d2}", -10); 
            CheckFormat("{0,1:d2}", -15); 
            CheckFormat("{0,1:d2}", -100); 
            CheckFormat("{0,1:d2}", -123); 
            CheckFormat("{0,1:d2}", -1024); 
            CheckFormat("{0,1:d3}", 0); 
            CheckFormat("{0,1:d3}", 5); 
            CheckFormat("{0,1:d3}", 10); 
            CheckFormat("{0,1:d3}", 15); 
            CheckFormat("{0,1:d3}", 100); 
            CheckFormat("{0,1:d3}", 123); 
            CheckFormat("{0,1:d3}", 1024); 
            CheckFormat("{0,1:d3}", -5); 
            CheckFormat("{0,1:d3}", -10); 
            CheckFormat("{0,1:d3}", -15); 
            CheckFormat("{0,1:d3}", -100); 
            CheckFormat("{0,1:d3}", -123); 
            CheckFormat("{0,1:d3}", -1024); 
            CheckFormat("{0,1:d4}", 0); 
            CheckFormat("{0,1:d4}", 5); 
            CheckFormat("{0,1:d4}", 10); 
            CheckFormat("{0,1:d4}", 15); 
            CheckFormat("{0,1:d4}", 100); 
            CheckFormat("{0,1:d4}", 123); 
            CheckFormat("{0,1:d4}", 1024); 
            CheckFormat("{0,1:d4}", -5); 
            CheckFormat("{0,1:d4}", -10); 
            CheckFormat("{0,1:d4}", -15); 
            CheckFormat("{0,1:d4}", -100); 
            CheckFormat("{0,1:d4}", -123); 
            CheckFormat("{0,1:d4}", -1024); 
        }

        [Fact]
        public void TestStringFormatD2()
        {
            CheckFormat("{0,2:d}", 0); 
            CheckFormat("{0,2:d}", 5); 
            CheckFormat("{0,2:d}", 10); 
            CheckFormat("{0,2:d}", 15); 
            CheckFormat("{0,2:d}", 100); 
            CheckFormat("{0,2:d}", 123); 
            CheckFormat("{0,2:d}", 1024); 
            CheckFormat("{0,2:d}", -5); 
            CheckFormat("{0,2:d}", -10); 
            CheckFormat("{0,2:d}", -15); 
            CheckFormat("{0,2:d}", -100); 
            CheckFormat("{0,2:d}", -123); 
            CheckFormat("{0,2:d}", -1024); 
            CheckFormat("{0,2:d1}", 0); 
            CheckFormat("{0,2:d1}", 5); 
            CheckFormat("{0,2:d1}", 10); 
            CheckFormat("{0,2:d1}", 15); 
            CheckFormat("{0,2:d1}", 100); 
            CheckFormat("{0,2:d1}", 123); 
            CheckFormat("{0,2:d1}", 1024); 
            CheckFormat("{0,2:d1}", -5); 
            CheckFormat("{0,2:d1}", -10); 
            CheckFormat("{0,2:d1}", -15); 
            CheckFormat("{0,2:d1}", -100); 
            CheckFormat("{0,2:d1}", -123); 
            CheckFormat("{0,2:d1}", -1024); 
            CheckFormat("{0,2:d2}", 0); 
            CheckFormat("{0,2:d2}", 5); 
            CheckFormat("{0,2:d2}", 10); 
            CheckFormat("{0,2:d2}", 15); 
            CheckFormat("{0,2:d2}", 100); 
            CheckFormat("{0,2:d2}", 123); 
            CheckFormat("{0,2:d2}", 1024); 
            CheckFormat("{0,2:d2}", -5); 
            CheckFormat("{0,2:d2}", -10); 
            CheckFormat("{0,2:d2}", -15); 
            CheckFormat("{0,2:d2}", -100); 
            CheckFormat("{0,2:d2}", -123); 
            CheckFormat("{0,2:d2}", -1024); 
            CheckFormat("{0,2:d3}", 0); 
            CheckFormat("{0,2:d3}", 5); 
            CheckFormat("{0,2:d3}", 10); 
            CheckFormat("{0,2:d3}", 15); 
            CheckFormat("{0,2:d3}", 100); 
            CheckFormat("{0,2:d3}", 123); 
            CheckFormat("{0,2:d3}", 1024); 
            CheckFormat("{0,2:d3}", -5); 
            CheckFormat("{0,2:d3}", -10); 
            CheckFormat("{0,2:d3}", -15); 
            CheckFormat("{0,2:d3}", -100); 
            CheckFormat("{0,2:d3}", -123); 
            CheckFormat("{0,2:d3}", -1024); 
            CheckFormat("{0,2:d4}", 0); 
            CheckFormat("{0,2:d4}", 5); 
            CheckFormat("{0,2:d4}", 10); 
            CheckFormat("{0,2:d4}", 15); 
            CheckFormat("{0,2:d4}", 100); 
            CheckFormat("{0,2:d4}", 123); 
            CheckFormat("{0,2:d4}", 1024); 
            CheckFormat("{0,2:d4}", -5); 
            CheckFormat("{0,2:d4}", -10); 
            CheckFormat("{0,2:d4}", -15); 
            CheckFormat("{0,2:d4}", -100); 
            CheckFormat("{0,2:d4}", -123); 
            CheckFormat("{0,2:d4}", -1024); 
        }

        [Fact]
        public void TestStringFormatD3()
        {
            CheckFormat("{0,3:d}", 0); 
            CheckFormat("{0,3:d}", 5); 
            CheckFormat("{0,3:d}", 10); 
            CheckFormat("{0,3:d}", 15); 
            CheckFormat("{0,3:d}", 100); 
            CheckFormat("{0,3:d}", 123); 
            CheckFormat("{0,3:d}", 1024); 
            CheckFormat("{0,3:d}", -5); 
            CheckFormat("{0,3:d}", -10); 
            CheckFormat("{0,3:d}", -15); 
            CheckFormat("{0,3:d}", -100); 
            CheckFormat("{0,3:d}", -123); 
            CheckFormat("{0,3:d}", -1024); 
            CheckFormat("{0,3:d1}", 0); 
            CheckFormat("{0,3:d1}", 5); 
            CheckFormat("{0,3:d1}", 10); 
            CheckFormat("{0,3:d1}", 15); 
            CheckFormat("{0,3:d1}", 100); 
            CheckFormat("{0,3:d1}", 123); 
            CheckFormat("{0,3:d1}", 1024); 
            CheckFormat("{0,3:d1}", -5); 
            CheckFormat("{0,3:d1}", -10); 
            CheckFormat("{0,3:d1}", -15); 
            CheckFormat("{0,3:d1}", -100); 
            CheckFormat("{0,3:d1}", -123); 
            CheckFormat("{0,3:d1}", -1024); 
            CheckFormat("{0,3:d2}", 0); 
            CheckFormat("{0,3:d2}", 5); 
            CheckFormat("{0,3:d2}", 10); 
            CheckFormat("{0,3:d2}", 15); 
            CheckFormat("{0,3:d2}", 100); 
            CheckFormat("{0,3:d2}", 123); 
            CheckFormat("{0,3:d2}", 1024); 
            CheckFormat("{0,3:d2}", -5); 
            CheckFormat("{0,3:d2}", -10); 
            CheckFormat("{0,3:d2}", -15); 
            CheckFormat("{0,3:d2}", -100); 
            CheckFormat("{0,3:d2}", -123); 
            CheckFormat("{0,3:d2}", -1024); 
            CheckFormat("{0,3:d3}", 0); 
            CheckFormat("{0,3:d3}", 5); 
            CheckFormat("{0,3:d3}", 10); 
            CheckFormat("{0,3:d3}", 15); 
            CheckFormat("{0,3:d3}", 100); 
            CheckFormat("{0,3:d3}", 123); 
            CheckFormat("{0,3:d3}", 1024); 
            CheckFormat("{0,3:d3}", -5); 
            CheckFormat("{0,3:d3}", -10); 
            CheckFormat("{0,3:d3}", -15); 
            CheckFormat("{0,3:d3}", -100); 
            CheckFormat("{0,3:d3}", -123); 
            CheckFormat("{0,3:d3}", -1024); 
            CheckFormat("{0,3:d4}", 0); 
            CheckFormat("{0,3:d4}", 5); 
            CheckFormat("{0,3:d4}", 10); 
            CheckFormat("{0,3:d4}", 15); 
            CheckFormat("{0,3:d4}", 100); 
            CheckFormat("{0,3:d4}", 123); 
            CheckFormat("{0,3:d4}", 1024); 
            CheckFormat("{0,3:d4}", -5); 
            CheckFormat("{0,3:d4}", -10); 
            CheckFormat("{0,3:d4}", -15); 
            CheckFormat("{0,3:d4}", -100); 
            CheckFormat("{0,3:d4}", -123); 
            CheckFormat("{0,3:d4}", -1024); 
        }

        [Fact]
        public void TestStringFormatD4()
        {
            CheckFormat("{0,4:d}", 0); 
            CheckFormat("{0,4:d}", 5); 
            CheckFormat("{0,4:d}", 10); 
            CheckFormat("{0,4:d}", 15); 
            CheckFormat("{0,4:d}", 100); 
            CheckFormat("{0,4:d}", 123); 
            CheckFormat("{0,4:d}", 1024); 
            CheckFormat("{0,4:d}", -5); 
            CheckFormat("{0,4:d}", -10); 
            CheckFormat("{0,4:d}", -15); 
            CheckFormat("{0,4:d}", -100); 
            CheckFormat("{0,4:d}", -123); 
            CheckFormat("{0,4:d}", -1024); 
            CheckFormat("{0,4:d1}", 0); 
            CheckFormat("{0,4:d1}", 5); 
            CheckFormat("{0,4:d1}", 10); 
            CheckFormat("{0,4:d1}", 15); 
            CheckFormat("{0,4:d1}", 100); 
            CheckFormat("{0,4:d1}", 123); 
            CheckFormat("{0,4:d1}", 1024); 
            CheckFormat("{0,4:d1}", -5); 
            CheckFormat("{0,4:d1}", -10); 
            CheckFormat("{0,4:d1}", -15); 
            CheckFormat("{0,4:d1}", -100); 
            CheckFormat("{0,4:d1}", -123); 
            CheckFormat("{0,4:d1}", -1024); 
            CheckFormat("{0,4:d2}", 0); 
            CheckFormat("{0,4:d2}", 5); 
            CheckFormat("{0,4:d2}", 10); 
            CheckFormat("{0,4:d2}", 15); 
            CheckFormat("{0,4:d2}", 100); 
            CheckFormat("{0,4:d2}", 123); 
            CheckFormat("{0,4:d2}", 1024); 
            CheckFormat("{0,4:d2}", -5); 
            CheckFormat("{0,4:d2}", -10); 
            CheckFormat("{0,4:d2}", -15); 
            CheckFormat("{0,4:d2}", -100); 
            CheckFormat("{0,4:d2}", -123); 
            CheckFormat("{0,4:d2}", -1024); 
            CheckFormat("{0,4:d3}", 0); 
            CheckFormat("{0,4:d3}", 5); 
            CheckFormat("{0,4:d3}", 10); 
            CheckFormat("{0,4:d3}", 15); 
            CheckFormat("{0,4:d3}", 100); 
            CheckFormat("{0,4:d3}", 123); 
            CheckFormat("{0,4:d3}", 1024); 
            CheckFormat("{0,4:d3}", -5); 
            CheckFormat("{0,4:d3}", -10); 
            CheckFormat("{0,4:d3}", -15); 
            CheckFormat("{0,4:d3}", -100); 
            CheckFormat("{0,4:d3}", -123); 
            CheckFormat("{0,4:d3}", -1024); 
            CheckFormat("{0,4:d4}", 0); 
            CheckFormat("{0,4:d4}", 5); 
            CheckFormat("{0,4:d4}", 10); 
            CheckFormat("{0,4:d4}", 15); 
            CheckFormat("{0,4:d4}", 100); 
            CheckFormat("{0,4:d4}", 123); 
            CheckFormat("{0,4:d4}", 1024); 
            CheckFormat("{0,4:d4}", -5); 
            CheckFormat("{0,4:d4}", -10); 
            CheckFormat("{0,4:d4}", -15); 
            CheckFormat("{0,4:d4}", -100); 
            CheckFormat("{0,4:d4}", -123); 
            CheckFormat("{0,4:d4}", -1024); 
        }

        [Fact]
        public void TestNumArgsNeeded()
        {
            var sf = new StringFormatter("{0}");
            Assert.Equal(1, sf.NumArgumentsNeeded);

            sf = new StringFormatter("{0}{1}");
            Assert.Equal(2, sf.NumArgumentsNeeded);

            sf = new StringFormatter("{0,3}");
            Assert.Equal(1, sf.NumArgumentsNeeded);

            sf = new StringFormatter("{0,3:d}");
            Assert.Equal(1, sf.NumArgumentsNeeded);

            sf = new StringFormatter("{0,3:d}{0}");
            Assert.Equal(1, sf.NumArgumentsNeeded);

            sf = new StringFormatter("{0,3:d}{1}");
            Assert.Equal(2, sf.NumArgumentsNeeded);

            sf = new StringFormatter("{0,3:d}{9}");
            Assert.Equal(10, sf.NumArgumentsNeeded);
        }
    }
}
