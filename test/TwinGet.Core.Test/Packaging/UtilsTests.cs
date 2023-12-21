// This file is licensed to you under MIT license.

namespace TwinGet.Core.Test.Packaging
{
    public class UtilsTests
    {
        [Theory]
        [InlineData("foo.bar", false)]
        [InlineData("foo.csproj", false)]
        [InlineData("foo.plcproj", true)]
        [InlineData("bar.nuspec", true)]
        public void IsSupportedFileType_ShouldWork(string file, bool expected)
        {
            var actual = Core.Packaging.Utils.IsSupportedFileType(file);

            actual.Should().Be(expected);
        }
    }
}
