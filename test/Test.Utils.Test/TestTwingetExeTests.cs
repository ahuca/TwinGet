// This file is licensed to you under MIT license.

using FluentAssertions;

namespace Test.Utils.Test
{
    public class TestTwingetExeTests
    {
        private readonly TestTwingetExe _sut = new();

        [Fact]
        public void Path_ShouldExists()
        {
            File.Exists(_sut.Path).Should().BeTrue();
            _sut.Path.Should().Contain(".exe");
        }
    }
}
