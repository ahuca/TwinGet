// This file is licensed to you under MIT license.

using NuGet.Configuration;
using Test.Utils;
using TwinGet.Core.Commands;
using TwinGet.Core.Packaging;
using TwinGet.TwincatInterface.Dto;
using TwinGet.TwincatInterface.Utils;
using Xunit.Abstractions;

namespace TwinGet.Core.Test.Packaging
{
    public class PackageServiceTests(ITestOutputHelper output)
    {
        private class TestData
        {
            public static TheoryData<PackCommandConfig> PackAsyncData()
            {
                return new()
                {
                    new PackCommandConfig()
                    {
                        ProvideSolutionPath = true
                    },
                    new PackCommandConfig()
                    {
                        ProvideSolutionPath = false
                    },
                };
            }
        }

        internal class PackCommandConfig
        {
            public bool ProvideSolutionPath { get; set; }
        }

        private readonly PackageService _sut = new();
        private readonly ITestOutputHelper _output = output;

        [Theory]
        [MemberData(nameof(TestData.PackAsyncData), MemberType = typeof(TestData))]
        internal async void PackAsync_WithPlcProjectFile_ShouldSucceed(PackCommandConfig config)
        {
            // Arange
            using TestProject testProject = new();
            _output.WriteLine(testProject.RootPath);
            var testPlcProject = testProject.GetPlcProjects().First();

            var packCommand = new PackCommand()
            {
                Path = testPlcProject.AbsolutePath,
                Solution = config.ProvideSolutionPath ? testProject.SolutionPath : string.Empty,
                OutputDirectory = testProject.RootPath,
            };

            var plcProjectData = TwincatUtils.DeserializeXmlFileToProjectData<PlcProjectData>(packCommand.Path);

            // Act
            var result = await _sut.PackAsync(packCommand);

            // Assert
            var exists = Directory.EnumerateFiles(packCommand.OutputDirectory, $"{plcProjectData.PropertyGroup.Title}*{NuGetConstants.PackageExtension}", SearchOption.TopDirectoryOnly).Any();
            result.Should().BeTrue();
            exists.Should().BeTrue();
        }
    }
}
