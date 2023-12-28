// This file is licensed to you under MIT license.

using NuGet.Configuration;
using Test.Utils;
using TwinGet.Core.Packaging;
using Xunit.Abstractions;

namespace TwinGet.Cli.FuncTest.Commands
{
    public class PackCommandTests(ITestOutputHelper output)
    {
        private static readonly TestTwingetExe _twingetExe = new();
        private static TestProject _testProject = new();
        private readonly CommandRunner _commandRunner = new(output);
        private readonly ITestOutputHelper _output = output;

        [Fact]
        public void Pack_WithManagedPlcProject_ShouldSucceed()
        {
            // Arrange
            var testPlcProject = _testProject.GetManagedPlcProjects().First();
            string[] args = [testPlcProject.AbsolutePath, "--solution", _testProject.SolutionPath];
            string expectedPackagePath = Path.Combine(
                _testProject.RootPath,
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
            var testPlcProject = _testProject
                .GetPlcProjects()
                .Where(x => !x.IsManagedLibrary)
                .First();
            string[] args = [testPlcProject.AbsolutePath, "--solution", _testProject.SolutionPath];

            // Act
            var result = RunPack(args);
            _output.WriteLine(result.AllOuput);

            // Assert
            result.ExitCode.Should().NotBe(0);
        }

        internal CommandRunnerResult RunPack(params string[] args)
        {
            string[] pack = ["pack"];
            var allArgs = pack.Concat(args);

            var result = _commandRunner.Run(
                _twingetExe.Path,
                _testProject.RootPath,
                string.Join(" ", allArgs)
            );

            return result;
        }
    }
}
