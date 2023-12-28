// This file is licensed to you under MIT license.

using NuGet.Configuration;
using Test.Utils;
using TwinGet.Core.Commands;
using TwinGet.Core.Packaging;
using TwinGet.TwincatInterface.Dto;
using TwinGet.TwincatInterface.Utils;
using Xunit.Abstractions;
using Task = System.Threading.Tasks.Task;

namespace TwinGet.Core.Test.Packaging
{
    public class PackageServiceTests
    {
        private readonly TestProject _testProject = new();
        private readonly PackageService _sut = new();
        private readonly ITestOutputHelper _output;

        public PackageServiceTests(ITestOutputHelper output)
        {
            _output = output;
            _output.WriteLine(_testProject.RootPath);
        }

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

        [Theory]
        [MemberData(nameof(TestData.PackAsyncData), MemberType = typeof(TestData))]
        internal async void PackAsync_WithValidParameters_ShouldSucceedAsync(PackCommandConfig config)
        {
            // Arange
            var testPlcProject = _testProject.GetManagedPlcProjects().First();
            var packCommand = new PackCommand()
            {
                Path = testPlcProject.AbsolutePath,
                Solution = config.ProvideSolutionPath ? _testProject.SolutionPath : string.Empty,
                OutputDirectory = _testProject.RootPath,
            };
            var plcProjectData = TwincatUtils.DeserializeXmlFileToProjectData<PlcProjectData>(packCommand.Path);

            // Act
            var result = await _sut.PackAsync(packCommand);

            // Assert
            var exists = Directory.EnumerateFiles(
                packCommand.OutputDirectory,
                $"{plcProjectData.PropertyGroup.Title}*{NuGetConstants.PackageExtension}",
                SearchOption.TopDirectoryOnly).Any();
            result.Should().BeTrue();
            exists.Should().BeTrue();
        }

        [Fact]
        public async void PackAsync_WithNoPath_ShouldThrowAsync()
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
        public async void PackAsync_WithNoOutputDirectory_ShouldThrowAsync()
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
    }
}
