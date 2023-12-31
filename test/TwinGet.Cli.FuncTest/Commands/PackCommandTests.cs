// This file is licensed to you under MIT license.

using NuGet.Configuration;
using Test.Utils;
using TwinGet.Core.Packaging;
using Xunit.Abstractions;

namespace TwinGet.Cli.FuncTest.Commands;

public class PackCommandTests(ITestOutputHelper output)
{
    private static readonly TestTwingetExe s_twingetExe = new();
    private static readonly TestProject s_testProject = new();
    private readonly CommandRunner _commandRunner = new(output);
    private readonly ITestOutputHelper _output = output;

    [Fact]
    public void Pack_WithManagedPlcProject_ShouldSucceed()
    {
        // Arrange
        var testPlcProject = s_testProject.GetManagedPlcProjects().First();
        string[] args = [testPlcProject.AbsolutePath, "--solution", s_testProject.SolutionPath];
        string expectedPackagePath = Path.Combine(
            s_testProject.RootPath,
            $"{testPlcProject.Title}{NuGetConstants.PackageExtension}"
        );

        // Act
        var result = RunPack(args);
        _output.WriteLine(result.AllOuput);

        // Assert
        result.ExitCode.Should().Be(0);
        result
            .AllOuput.Should()
            .Contain(PackagingStrings.PackSuccess.Replace("{Path}", expectedPackagePath));
        File.Exists(expectedPackagePath).Should().BeTrue();
    }

    [Fact]
    public void Pack_WithUnmanagedPlcProject_ShouldFail()
    {
        // Arrange
        var testPlcProject = s_testProject.GetPlcProjects().Where(x => !x.IsManagedLibrary).First();
        string[] args = [testPlcProject.AbsolutePath, "--solution", s_testProject.SolutionPath];

        // Act
        var result = RunPack(args);
        _output.WriteLine(result.AllOuput);

        // Assert
        result.ExitCode.Should().NotBe(0);
        result.AllOuput.Should().Contain("The specified library is not a managed library.");
        result
            .AllOuput.Should()
            .Contain(
                PackagingErrors.FailedToSavePlcLibrary.Replace(
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
