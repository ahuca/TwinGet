// This file is licensed to you under MIT license.

using TwinGet.TwincatInterface.ProjectFileUtils;
using Xunit.Abstractions;
using static TwinGet.TwincatInterface.Test.ProjectFileUtils.TestData;

namespace TwinGet.TwincatInterface.Test.ProjectFileUtils;

public class TcSmProjectFileHelperTests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;

    [Fact]
    public void Guid_ShouldWork()
    {
        // Arrange
        using var testDirectory = TestDirectory.Create();
        string twincatProjectPath = Path.Combine(
            testDirectory.Path,
            $"TestPlcProject1{TwincatConstants.TwincatXaeProjectExtension}"
        );
        File.Create(twincatProjectPath).Close();
        File.WriteAllText(twincatProjectPath, TwincatFileContent);
        string expected = TwincatProjectGuid;

        // Act
        string actual = TcSmProjectFileHelper.Create(twincatProjectPath).Guid();

        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public async Task GuidAsync_ShouldWorkAsync()
    {
        // Arrange
        using var testDirectory = TestDirectory.Create();
        string twincatProjectPath = Path.Combine(
            testDirectory.Path,
            $"TestPlcProject1{TwincatConstants.TwincatXaeProjectExtension}"
        );
        File.Create(twincatProjectPath).Close();
        File.WriteAllText(twincatProjectPath, TwincatFileContent);
        string expected = TwincatProjectGuid;

        // Act
        string actual = await TcSmProjectFileHelper.Create(twincatProjectPath).GuidAsync();

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData(6, 5, true)]
    [InlineData(5, 5, false)]
    [InlineData(3, 5, false)]
    public void GetParentSolutionFile_Specs(int actualDepth, int searchDepth, bool resultIsEmpty)
    {
        // Arrange
        using var testDirectory = TestDirectory.Create();
        // Place solution dir at the test directory.
        string solutionDir = testDirectory.Path;
        // Place TwinCAT project file actualDepth levels below the
        string twincatProjectDir = solutionDir;
        for (int i = 0; i < actualDepth - 1; i++)
        {
            twincatProjectDir = Path.Combine(twincatProjectDir, (i + 1).ToString());
        }
        // Create solution file
        string solutionPath = Path.Combine(
            solutionDir,
            $"solution{TwincatConstants.SolutionExtension}"
        );
        _output.WriteLine($"Solution file: {solutionPath}");
        File.Create(solutionPath).Close();
        File.WriteAllText(solutionPath, SolutionFileContent);
        // Create TwinCAT project file
        Directory.CreateDirectory(twincatProjectDir);
        string twincatProjectPath = Path.Combine(
            twincatProjectDir,
            $"TestTwincatProject1{TwincatConstants.TwincatXaeProjectExtension}"
        );
        File.Create(twincatProjectPath).Close();
        File.WriteAllText(twincatProjectPath, TwincatFileContent);
        _output.WriteLine($"TwinCAT project file: {twincatProjectPath}");

        // Act
        string actual = TcSmProjectFileHelper
            .Create(twincatProjectPath)
            .GetParentSolutionFile(searchDepth);

        // Assert
        bool foundParent = string.IsNullOrEmpty(actual);
        foundParent.Should().Be(resultIsEmpty);
        string expected = resultIsEmpty ? string.Empty : solutionPath;
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData(6, 5, true)]
    [InlineData(5, 5, false)]
    [InlineData(3, 5, false)]
    public async Task GetParentSolutionFile_SpecsAsync(
        int actualDepth,
        int searchDepth,
        bool resultIsEmpty
    )
    {
        // Arrange
        using var testDirectory = TestDirectory.Create();
        // Place solution dir at the test directory.
        string solutionDir = testDirectory.Path;
        // Place TwinCAT project file actualDepth levels below the
        string twincatProjectDir = solutionDir;
        for (int i = 0; i < actualDepth - 1; i++)
        {
            twincatProjectDir = Path.Combine(twincatProjectDir, (i + 1).ToString());
        }
        // Create solution file
        string solutionPath = Path.Combine(
            solutionDir,
            $"solution{TwincatConstants.SolutionExtension}"
        );
        _output.WriteLine($"Solution file: {solutionPath}");
        File.Create(solutionPath).Close();
        File.WriteAllText(solutionPath, SolutionFileContent);
        // Create TwinCAT project file
        Directory.CreateDirectory(twincatProjectDir);
        string twincatProjectPath = Path.Combine(
            twincatProjectDir,
            $"TestTwincatProject1{TwincatConstants.TwincatXaeProjectExtension}"
        );
        File.Create(twincatProjectPath).Close();
        File.WriteAllText(twincatProjectPath, TwincatFileContent);
        _output.WriteLine($"TwinCAT project file: {twincatProjectPath}");

        // Act
        string actual = await TcSmProjectFileHelper
            .Create(twincatProjectPath)
            .GetParentSolutionFileAsync(searchDepth);

        // Assert
        bool foundParent = string.IsNullOrEmpty(actual);
        foundParent.Should().Be(resultIsEmpty);
        string expected = resultIsEmpty ? string.Empty : solutionPath;
        actual.Should().Be(expected);
    }
}
