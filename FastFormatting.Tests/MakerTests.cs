// © Microsoft Corporation. All rights reserved.

namespace FastFormatting.Tests
{
    using System;
    using Xunit;
    using Xunit.Abstractions;

    public class MakerTests
    {
        private readonly ITestOutputHelper _output;

        public MakerTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void TestReadOnlySpan()
        {
            var span = "Test".AsSpan();
            var sm = new StringMaker();
            sm.Append(span);
            var str = sm.ExtractString();
            Assert.Equal("Test", str);
        }
    }
}
