// This file is licensed to you under MIT license.

using NuGet.Configuration;
using Test.Utils;
using TwinGet.Core.Packaging;
using TwinGet.TwincatInterface;
using Xunit.Abstractions;
using CoreErrorStrings = TwinGet.Core.Packaging.ErrorStrings;

namespace TwinGet.Cli.FuncTest.Commands;

public class PackCommandTests(ITestOutputHelper output)
{
    private static readonly TestTwingetExe s_twingetExe = new();
    private static readonly TestProject s_testProject = new();
    private readonly CommandRunner _commandRunner = new(output);
    private readonly ITestOutputHelper _output = output;

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Pack_WithManagedPlcProject_ShouldSucceed(bool provideSolution)
    {
        // Arrange
        var testPlcProject = s_testProject.GetManagedPlcProjects().First();
        string[] args = [testPlcProject.AbsolutePath];
        if (provideSolution)
        {
            args = [.. args, .. new string[2] { "--solution", s_testProject.SolutionPath }];
        }

        string expectedPackagePath = Path.Combine(
            s_testProject.RootPath,
            $"{testPlcProject.Title}{NuGetConstants.PackageExtension}"
        );

        // Act
        var result = RunPack(args);
        _output.WriteLine(result.AllOutput);

        // Assert
        result.ExitCode.Should().Be(0);
        result
            .AllOutput.Should()
            .Contain(OtherStrings.PackSuccess.Replace("{Path}", expectedPackagePath));
        if (!provideSolution)
        {
            result
                .AllOutput.Should()
                .Contain(SuggestionStrings.SpecifySolutionFileForBetterPerformance);
        }

        File.Exists(expectedPackagePath).Should().BeTrue();
    }

    [Fact]
    public void Pack_WithUnmanagedPlcProject_ShouldFail()
    {
        // Arrange
        var testPlcProject = s_testProject
            .GetPlcProjects()
            .Where(x => !x.IsManagedLibrary())
            .First();
        string[] args = [testPlcProject.AbsolutePath, "--solution", s_testProject.SolutionPath];

        // Act
        var result = RunPack(args);
        _output.WriteLine(result.AllOutput);

        // Assert
        result.ExitCode.Should().NotBe(0);
        result.AllOutput.Should().Contain("The specified library is not a managed library.");
        result
            .AllOutput.Should()
            .Contain(
                CoreErrorStrings.FailedToSavePlcLibrary.Replace(
                    "{Path}",
                    testPlcProject.AbsolutePath
                )
            );
    }

    internal CommandRunnerResult RunPack(params string[] args)
    {
        string[] pack = ["pack"];
        var allArgs = pack.Concat(args);

        var result = _commandRunner.Run(
            s_twingetExe.Path,
            s_testProject.RootPath,
            string.Join(" ", allArgs)
        );

        return result;
    }
}
