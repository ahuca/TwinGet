// This file is licensed to you under MIT license.

using TwinGet.Core.Packaging;

namespace TwinGet.Cli.Test;

public class PackStrategyFactoryTests
{
    private readonly PackStrategyFactory _sut = new(null);

    [Theory]
    [InlineData("foo.plcproj", typeof(PlcProjectPackStrategy))]
    //[InlineData("bar.nuspec", typeof(NuspecPackStrategy))] // TODO: Uncomment this test when nuspec is supported.
    public void GetStrategy_OfSupportedFileTypes_ShouldSucceed(string file, Type? expected)
    {
        var actual = _sut.CreateStrategy(file).GetType();

        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("foo.bar")]
    [InlineData("foo.csproj")]
    public void GetStrategy_OfUnsupportedFileTypes_ShouldThrow(string file)
    {
        var act = () => _sut.CreateStrategy(file);

        act.Should().Throw<NotSupportedException>();
    }
}
