// This file is licensed to you under MIT license.

using NuGet.Configuration;
using Test.Utils;
using TwinGet.Core.Commands;
using TwinGet.Core.Packaging;
using TwinGet.TwincatInterface.Dto;
using TwinGet.TwincatInterface.Utils;
using Xunit.Abstractions;
using Task = System.Threading.Tasks.Task;

namespace TwinGet.Core.Test.Packaging;

public class PlcProjectPackStrategyTests
{
    private readonly TestProject _testProject = new();
    private readonly PlcProjectPackStrategy _sut = new(null);
    private readonly ITestOutputHelper _output;

    public PlcProjectPackStrategyTests(ITestOutputHelper output)
    {
        _output = output;
        _output.WriteLine(_testProject.RootPath);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Pack_WithValidParameters_ShouldSucceed(bool provideSolution)
    {
        // Arange
        var testPlcProject = _testProject.GetManagedPlcProjects().First();
        var packCommand = new PackCommand()
        {
            Path = testPlcProject.AbsolutePath,
            Solution = provideSolution ? _testProject.SolutionPath : string.Empty,
            OutputDirectory = _testProject.RootPath,
        };
        var plcProjectData = TwincatUtils.DeserializeXmlFileToProjectData<PlcProjectData>(
            packCommand.Path
        );

        // Act
        var result = _sut.Pack(packCommand);

        // Assert
        var exists = Directory
            .EnumerateFiles(
                packCommand.OutputDirectory,
                $"{plcProjectData.PropertyGroup.Title}*{NuGetConstants.PackageExtension}",
                SearchOption.TopDirectoryOnly
            )
            .Any();
        result.Should().BeTrue();
        exists.Should().BeTrue();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task PackAsync_WithValidParameters_ShouldSucceedAsync(bool provideSolution)
    {
        // Arange
        var testPlcProject = _testProject.GetManagedPlcProjects().First();
        var packCommand = new PackCommand()
        {
            Path = testPlcProject.AbsolutePath,
            Solution = provideSolution ? _testProject.SolutionPath : string.Empty,
            OutputDirectory = _testProject.RootPath,
        };
        var plcProjectData = TwincatUtils.DeserializeXmlFileToProjectData<PlcProjectData>(
            packCommand.Path
        );

        // Act
        var result = await _sut.PackAsync(packCommand);

        // Assert
        var exists = Directory
            .EnumerateFiles(
                packCommand.OutputDirectory,
                $"{plcProjectData.PropertyGroup.Title}*{NuGetConstants.PackageExtension}",
                SearchOption.TopDirectoryOnly
            )
            .Any();
        result.Should().BeTrue();
        exists.Should().BeTrue();
    }

    [Fact]
    public void Pack_WithNoPath_ShouldThrow()
    {
        // Arrange
        var packCommand = new PackCommand()
        {
            Path = string.Empty,
            Solution = _testProject.RootPath,
            OutputDirectory = _testProject.RootPath,
        };

        // Act
        Action pack = () => _sut.Pack(packCommand);

        // Assert
        pack.Should().Throw<ArgumentException>();
    }

    [Fact]
    public async Task PackAsync_WithNoPath_ShouldThrowAsync()
    {
        // Arrange
        var packCommand = new PackCommand()
        {
            Path = string.Empty,
            Solution = _testProject.RootPath,
            OutputDirectory = _testProject.RootPath,
        };

        // Act
        Task packAsync() => _sut.PackAsync(packCommand);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(packAsync);
    }

    [Fact]
    public void Pack_WithNoOutputDirectory_ShouldThrow()
    {
        // Arrange
        var testPlcProject = _testProject.GetManagedPlcProjects().First();
        var packCommand = new PackCommand()
        {
            Path = testPlcProject.AbsolutePath,
            Solution = _testProject.RootPath,
            OutputDirectory = string.Empty,
        };

        // Act
        Action pack = () => _sut.Pack(packCommand);

        // Assert
        pack.Should().Throw<ArgumentException>();
    }

    [Fact]
    public async Task PackAsync_WithNoOutputDirectory_ShouldThrowAsync()
    {
        // Arrange
        var testPlcProject = _testProject.GetManagedPlcProjects().First();
        var packCommand = new PackCommand()
        {
            Path = testPlcProject.AbsolutePath,
            Solution = _testProject.RootPath,
            OutputDirectory = string.Empty,
        };

        // Act
        Task packAsync() => _sut.PackAsync(packCommand);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(packAsync);
    }

    //[Fact]
    //public async Task PackAsync_WithNoSolutionFile_ShouldThrowAsync()
    //{
    //    // Arrange
    //    using var testDirectory = TestDirectory.Create();
    //    string plcProjectPath = Path.Combine(testDirectory.Path, $"TestPlcProject{TwincatConstants.PlcProjectExtension}");
    //    File.Create(plcProjectPath).Close();
    //    var packCommand = new PackCommand()
    //    {
    //        Path = plcProjectPath,
    //        Solution = string.Empty,
    //        OutputDirectory = testDirectory.Path,
    //    };

    //    // Act
    //    Task pack() => _sut.PackAsync(packCommand);

    //    // Assert
    //    await Assert.ThrowsAsync<PackagingException>(pack);
    //}
}
