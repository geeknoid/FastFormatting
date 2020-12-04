// © Microsoft Corporation. All rights reserved.

namespace FastFormatting.Tests
{
    using System;
    using System.Text;
    using Xunit;

    public class FormatTests
    {
        [Fact]
        public void Basic()
        {
            var sf = new StringFormatter("Hello {0}");
            var str = sf.Format(null, "Bob");
            Assert.Equal("Hello Bob", str);
        }


        private void CheckFormat(string format, params object[] args)
        {
            string? expectedResult = null;
            Exception? expectedException = null;

            string? actualResult = null;
            Exception? actualException = null;

            try
            {
                expectedResult = String.Format(format, args);
            }
            catch (FormatException ex)
            {
                expectedException = ex;
            }

            try
            {
                var sf = new StringFormatter(format);
                actualResult = sf.Format(null, args);
            }
            catch (FormatException ex)
            {
                actualException = ex;
            }

            Assert.Equal(expectedResult, actualResult);
            Assert.Equal(expectedException, actualException);
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

        sealed class TestFormat : IFormattable
        {
            readonly string m_expectedFormat;
            readonly string m_result;

            public TestFormat(string expectedFormat, string result)
            {
                m_expectedFormat = expectedFormat;
                m_result = result;
            }

            public string ToString(string? format, IFormatProvider? provider)
            {
                return format == m_expectedFormat ? m_result : string.Empty;
            }
        }

        [Fact]
        public void StringFormatChecks()
        {
            CheckFormat("{0}", 1); 
            CheckFormat("{0}{{}}{1}", 1, 2); 

            var tf = new TestFormat("{1", "XXX");
            CheckFormat("a{0:{{1}b", tf); 

            tf = new TestFormat("z{1", "XXX");
            CheckFormat("a{0:z{{1}b", tf); 

            tf = new TestFormat("z{{1", "XXX");
            CheckFormat("a{0:z{{{{1}b", tf); 

            tf = new TestFormat("1}", "XXX");
            CheckFormat("a{0:1}}}b", tf); 

            tf = new TestFormat("1}z", "XXX");
            CheckFormat("a{0:1}}z}b", tf); 

            tf = new TestFormat("1}z}", "XXX");
            CheckFormat("a{0:1}}z}}}b", tf); 

            // tests for field width

            CheckFormat("{0,0}", ""); 
            CheckFormat("{0,1}", ""); 
            CheckFormat("{0,-1}", ""); 
            CheckFormat("{0,2}", ""); 
            CheckFormat("{0,-2}", ""); 
            CheckFormat("{0,3}", ""); 
            CheckFormat("{0,-3}", ""); 

            CheckFormat("{0,0}", "X"); 
            CheckFormat("{0,1}", "X"); 
            CheckFormat("{0,-1}", "X"); 
            CheckFormat("{0,2}", "X"); 
            CheckFormat("{0,-2}", "X"); 
            CheckFormat("{0,3}", "X"); 
            CheckFormat("{0,-3}", "X"); 

            CheckFormat("{0,0}", "XY"); 
            CheckFormat("{0,1}", "XY"); 
            CheckFormat("{0,-1}", "XY"); 
            CheckFormat("{0,2}", "XY"); 
            CheckFormat("{0,-2}", "XY"); 
            CheckFormat("{0,3}", "XY"); 
            CheckFormat("{0,-3}", "XY"); 

            CheckFormat("{0,0}", "XYZ"); 
            CheckFormat("{0,1}", "XYZ"); 
            CheckFormat("{0,-1}", "XYZ"); 
            CheckFormat("{0,2}", "XYZ"); 
            CheckFormat("{0,-2}", "XYZ"); 
            CheckFormat("{0,3}", "XYZ"); 
            CheckFormat("{0,-3}", "XYZ"); 

            CheckFormat("{0,*1}", "", 0); 
            CheckFormat("{0,*1}", "", 1); 
            CheckFormat("{0,*1}", "", -1); 
            CheckFormat("{0,*1}", "", 2); 
            CheckFormat("{0,*1}", "", -2); 
            CheckFormat("{0,*1}", "", 3); 
            CheckFormat("{0,*1}", "", -3); 

            CheckFormat("{0,*1}", "X", 0); 
            CheckFormat("{0,*1}", "X", 1); 
            CheckFormat("{0,*1}", "X", -1); 
            CheckFormat("{0,*1}", "X", 2); 
            CheckFormat("{0,*1}", "X", -2); 
            CheckFormat("{0,*1}", "X", 3); 
            CheckFormat("{0,*1}", "X", -3); 

            CheckFormat("{0,*1}", "XY", 0); 
            CheckFormat("{0,*1}", "XY", 1); 
            CheckFormat("{0,*1}", "XY", -1); 
            CheckFormat("{0,*1}", "XY", 2); 
            CheckFormat("{0,*1}", "XY", -2); 
            CheckFormat("{0,*1}", "XY", 3); 
            CheckFormat("{0,*1}", "XY", -3); 

            CheckFormat("{0,*1}", "XYZ", 0); 
            CheckFormat("{0,*1}", "XYZ", 1); 
            CheckFormat("{0,*1}", "XYZ", -1); 
            CheckFormat("{0,*1}", "XYZ", 2); 
            CheckFormat("{0,*1}", "XYZ", -2); 
            CheckFormat("{0,*1}", "XYZ", 3); 
            CheckFormat("{0,*1}", "XYZ", -3); 
        }
    }
}
