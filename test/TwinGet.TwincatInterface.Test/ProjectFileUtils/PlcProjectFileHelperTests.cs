// This file is licensed to you under MIT license.

using TwinGet.TwincatInterface.ProjectFileUtils;
using Xunit.Abstractions;
using static TwinGet.TwincatInterface.Test.ProjectFileUtils.TestData;

namespace TwinGet.TwincatInterface.Test.ProjectFileUtils;

public class PlcProjectFileHelperTests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;

    [Fact]
    public void Guid_ShouldWork()
    {
        // Arrange
        using var testDirectory = TestDirectory.Create();
        string plcProjectPath = Path.Combine(
            testDirectory.Path,
            $"TestPlcProject1{TwincatConstants.PlcProjectExtension}"
        );
        File.Create(plcProjectPath).Close();
        File.WriteAllText(plcProjectPath, PlcFileContent);
        string expected = PlcProjectGuid;

        // Act
        string actual = PlcProjectFileHelper.Create(plcProjectPath).Guid();

        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public async Task GuidAsync_ShouldWorkAsync()
    {
        // Arrange
        using var testDirectory = TestDirectory.Create();
        string plcProjectPath = Path.Combine(
            testDirectory.Path,
            $"TestPlcProject1{TwincatConstants.PlcProjectExtension}"
        );
        File.Create(plcProjectPath).Close();
        File.WriteAllText(plcProjectPath, PlcFileContent);
        string expected = PlcProjectGuid;

        // Act
        string actual = await PlcProjectFileHelper.Create(plcProjectPath).GuidAsync();

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData(6, 5, true)]
    [InlineData(5, 5, false)]
    [InlineData(3, 5, false)]
    public void GetParentTwincatFile_Specs(int actualDepth, int searchDepth, bool resultIsEmpty)
    {
        // Arrange
        using var testDirectory = TestDirectory.Create();
        // Place TwinCAT project dir at the test directory.
        string twincatProjectDir = testDirectory.Path;
        // Place PLC project file actualDepth levels below the
        string plcProjectDir = twincatProjectDir;
        for (int i = 0; i < actualDepth - 1; i++)
        {
            plcProjectDir = Path.Combine(plcProjectDir, (i + 1).ToString());
        }
        // Create TwinCAT project file
        string twincatProjectPath = Path.Combine(
            testDirectory.Path,
            $"TestTwincatProject1{TwincatConstants.TwincatXaeProjectExtension}"
        );
        _output.WriteLine($"TwinCAT project file: {twincatProjectPath}");
        File.Create(twincatProjectPath).Close();
        File.WriteAllText(twincatProjectPath, TwincatFileContent);
        // Create PLC project file
        Directory.CreateDirectory(plcProjectDir);
        string plcProjectPath = Path.Combine(
            plcProjectDir,
            $"TestPlcProject1{TwincatConstants.PlcProjectExtension}"
        );
        File.Create(plcProjectPath).Close();
        File.WriteAllText(plcProjectPath, PlcFileContent);
        _output.WriteLine($"PLC project file: {plcProjectPath}");

        // Act
        string actual = PlcProjectFileHelper.Create(plcProjectPath).GetParentTwincatFile(searchDepth);

        // Assert
        bool foundParent = string.IsNullOrEmpty(actual);
        foundParent.Should().Be(resultIsEmpty);
        string expected = resultIsEmpty ? string.Empty : twincatProjectPath;
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData(6, 5, true)]
    [InlineData(5, 5, false)]
    [InlineData(3, 5, false)]
    public async Task GetParentTwincatFileAsync_SpecsAsync(
        int actualDepth,
        int searchDepth,
        bool resultIsEmpty
    )
    {
        // Arrange
        using var testDirectory = TestDirectory.Create();
        // Place TwinCAT project dir at the test directory.
        string twincatProjectDir = testDirectory.Path;
        // Place PLC project file actualDepth levels below the
        string plcProjectDir = twincatProjectDir;
        for (int i = 0; i < actualDepth - 1; i++)
        {
            plcProjectDir = Path.Combine(plcProjectDir, (i + 1).ToString());
        }
        // Create TwinCAT project file
        string twincatProjectPath = Path.Combine(
            twincatProjectDir,
            $"TestTwincatProject1{TwincatConstants.TwincatXaeProjectExtension}"
        );
        _output.WriteLine($"TwinCAT project file: {twincatProjectPath}");
        File.Create(twincatProjectPath).Close();
        File.WriteAllText(twincatProjectPath, TwincatFileContent);
        // Create PLC project file
        Directory.CreateDirectory(plcProjectDir);
        string plcProjectPath = Path.Combine(
            plcProjectDir,
            $"TestPlcProject1{TwincatConstants.PlcProjectExtension}"
        );
        File.Create(plcProjectPath).Close();
        File.WriteAllText(plcProjectPath, PlcFileContent);
        _output.WriteLine($"PLC project file: {plcProjectPath}");

        // Act
        string actual = await PlcProjectFileHelper.Create(plcProjectPath).GetParentTwincatFileAsync(searchDepth);

        // Assert
        bool foundParent = string.IsNullOrEmpty(actual);
        foundParent.Should().Be(resultIsEmpty);
        string expected = resultIsEmpty ? string.Empty : twincatProjectPath;
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData(6, 5, true)]
    [InlineData(5, 5, false)]
    [InlineData(3, 5, false)]
    public async Task GetParentSolutionFileAsync_SpecsAsync(
        int actualDepth,
        int searchDepth,
        bool resultIsEmpty
    )
    {
        // Arrange
        using var testDirectory = TestDirectory.Create();
        string solutionDir = testDirectory.Path; // Place solution file at the test directory.
        // Place TwinCAT project file actualDepth levels below the solution file.
        string twincatProjectDir = solutionDir;
        int depth = 0;
        for (; depth < actualDepth - 1; depth++)
        {
            twincatProjectDir = Path.Combine(twincatProjectDir, (depth + 1).ToString());
        }
        // Place the PLC project file actualDepth levels below the solution file.
        string plcProjectDir = twincatProjectDir;
        for (; depth < 2 * (actualDepth - 1); depth++)
        {
            plcProjectDir = Path.Combine(plcProjectDir, (depth + 1).ToString());
        }
        // Create solution file
        string solutionPath = Path.Combine(solutionDir, $"solution{TwincatConstants.SolutionExtension}");
        File.Create(solutionPath).Close();
        File.WriteAllText(solutionPath, SolutionFileContent);
        _output.WriteLine($"TwinCAT project file: {solutionPath}");
        // Create TwinCAT project file
        Directory.CreateDirectory(plcProjectDir);
        string twincatProjectPath = Path.Combine(
            twincatProjectDir,
            $"TestTwincatProject1{TwincatConstants.TwincatXaeProjectExtension}"
        );
        _output.WriteLine($"TwinCAT project file: {twincatProjectPath}");
        File.Create(twincatProjectPath).Close();
        File.WriteAllText(twincatProjectPath, TwincatFileContent);
        // Create PLC project file
        Directory.CreateDirectory(plcProjectDir);
        string plcProjectPath = Path.Combine(
            plcProjectDir,
            $"TestPlcProject1{TwincatConstants.PlcProjectExtension}"
        );
        File.Create(plcProjectPath).Close();
        File.WriteAllText(plcProjectPath, PlcFileContent);
        _output.WriteLine($"PLC project file: {plcProjectPath}");

        // Act
        string actual = await PlcProjectFileHelper.Create(plcProjectPath).GetParentSolutionFileAsync(searchDepth);

        // Assert
        bool foundParent = string.IsNullOrEmpty(actual);
        foundParent.Should().Be(resultIsEmpty);
        string expected = resultIsEmpty ? string.Empty : solutionPath;
        actual.Should().Be(expected);
    }
}
