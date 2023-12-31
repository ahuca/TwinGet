// This file is licensed to you under MIT license.

using Xunit.Abstractions;
using static TwinGet.TwincatInterface.TwincatConstants;

namespace TwinGet.TwincatInterface.Test;

public class AutomationInterfaceTests(ITestOutputHelper output) : IDisposable
{
    private readonly ITestOutputHelper _output = output;
    private bool _disposedValue;

    [Fact]
    public void PathExists_ShouldSupportRelativePath()
    {
        Path.Exists(@$"TestTwincatProject\TestTwincatProject{TwincatConstants.SolutionExtension}")
            .Should()
            .BeTrue();
    }

    [StaFact]
    public void ProgId_ShouldNotBeNullOrEmpty()
    {
        using AutomationInterface sut = new();

        sut.ProgId.Should().NotBeNullOrEmpty();
    }

    [StaFact]
    public void ProgId_ShouldBeValid()
    {
        using AutomationInterface sut = new();

        SupportedProgIds.Should().Contain(sut.ProgId);
    }

    [StaFact]
    public void LoadSolution_WithValidPath_ShouldLoadSuccessfully()
    {
        using TestProject project = new();
        using AutomationInterface sut = new();

        sut.LoadSolution(project.SolutionPath);

        sut.LoadedSolutionFile.Should().Be(project.SolutionPath);
        sut.IsSolutionOpen.Should().BeTrue();
    }

    [StaFact]
    public void LoadSolution_WithInvalidPath_ShouldThrow()
    {
        using AutomationInterface sut = new();

        string invalidSolution = $"{Guid.NewGuid()}{TwincatConstants.SolutionExtension}";
        Action loadSolution = () => sut.LoadSolution(invalidSolution);

        loadSolution.Should().Throw<FileNotFoundException>();
        sut.IsSolutionOpen.Should().BeFalse();
    }

    [StaFact]
    public void LoadSolution_WithEmptyPath_ShouldThrow()
    {
        using AutomationInterface sut = new();

        Action loadSolution = () => sut.LoadSolution(string.Empty);

        loadSolution.Should().Throw<ArgumentException>();
        sut.IsSolutionOpen.Should().BeFalse();
    }

    [StaFact]
    public void GetPlcProjects_WithASolutionOpened_ShouldGetAll()
    {
        using TestProject testProject = new();
        _output.WriteLine(testProject.RootPath);
        using AutomationInterface sut = new();

        sut.LoadSolution(testProject.SolutionPath);

        IEnumerable<string> expected = testProject
            .TwincatProjects.SelectMany(t => t.PlcProjects)
            .Select(p => p.AbsolutePath);

        IEnumerable<string> actual = sut.GetPlcProjects().Select(p => p.AbsolutePath);

        expected.Should().NotBeNullOrEmpty(); // Make sure the test project has PLC projects.
        actual.Should().BeEquivalentTo(expected);
    }

    [StaFact]
    public void TwincatProjects_ShouldBeConsistent()
    {
        using TestProject testProject = new();
        _output.WriteLine(testProject.RootPath);
        using AutomationInterface sut = new();

        sut.LoadSolution(testProject.SolutionPath);

        IEnumerable<string> expected = testProject.TwincatProjects.Select(p => p.AbsolutePath);

        IEnumerable<string> actual = sut.TwincatProjects.Select(p => p.FullName);

        expected.Should().NotBeNullOrEmpty(); // Make sure the test project has TwinCAT projects.
        actual.Should().BeEquivalentTo(expected);
    }

    [StaFact]
    public void SavePlcProject_WithValidParams_ShouldSucceed()
    {
        // Arrange
        using TestProject testProject = new();
        _output.WriteLine(testProject.RootPath);
        using AutomationInterface sut = new();

        string outputDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        _output.WriteLine(outputDir);

        TestPlcProject plcProject = testProject
            .TwincatProjects.SelectMany(t => t.PlcProjects)
            .First();
        string expected = Path.Combine(
            outputDir,
            $"{plcProject.Title}{TwincatPlcLibraryExtension}"
        );

        // Act
        var result = sut.SavePlcProject(
            plcProject.AbsolutePath,
            outputDir,
            testProject.SolutionPath
        );

        // Assert
        File.Exists(expected).Should().BeTrue();
        result.Should().NotBeNull();
        result.Should().Contain(expected);
    }

    [StaFact]
    public void SavePlcProject_WithoutSolutionPath_ShouldSucceed()
    {
        // Arrange
        using TestProject testProject = new();
        _output.WriteLine(testProject.RootPath);
        using AutomationInterface sut = new();

        string outputDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        _output.WriteLine(outputDir);

        TestPlcProject plcProject = testProject
            .TwincatProjects.SelectMany(t => t.PlcProjects)
            .First();
        string expected = Path.Combine(
            outputDir,
            $"{plcProject.Title}{TwincatPlcLibraryExtension}"
        );

        // Act
        var result = sut.SavePlcProject(plcProject.AbsolutePath, outputDir);

        // Assert
        File.Exists(expected).Should().BeTrue();
        result.Should().NotBeNull();
        result.Should().Contain(expected);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing) { }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            _disposedValue = true;
        }
    }

    ~AutomationInterfaceTests()
    {
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
