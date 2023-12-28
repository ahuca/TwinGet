// This file is licensed to you under MIT license.

using FluentValidation;
using Moq;
using Test.Utils;
using TwinGet.Core.Commands;
using TwinGet.Core.Packaging;
using Xunit.Abstractions;

namespace TwinGet.Core.Test.Commands
{
    public class PackCommandHanderTests
    {
        private readonly TestProject _testProject = new();
        private readonly PackCommand _command;
        private readonly Mock<IValidator<PackCommand>> _validator = new();
        private readonly Mock<IPackageService> _packageService = new();
        private readonly PackCommandHander _sut;
        private readonly ITestOutputHelper _output;

        public PackCommandHanderTests(ITestOutputHelper output)
        {
            // We mock validation so that it always pass.
            Mock<FluentValidation.Results.ValidationResult> validationResult = new();
            validationResult.Setup(x => x.IsValid).Returns(true);
            _validator.Setup(v => v.ValidateAsync(It.IsAny<PackCommand>(), default)).ReturnsAsync(validationResult.Object);

            // Setup SUT.
            _sut = new(_validator.Object, _packageService.Object);

            // Template the pack command. Test methods can still override needed properties.
            _command = new()
            {
                Path = _testProject.GetManagedPlcProjects().First().AbsolutePath,
                Solution = _testProject.SolutionPath,
                OutputDirectory = _testProject.RootPath,
            };

            _output = output;
            Directory.SetCurrentDirectory(_testProject.RootPath);
            _output.WriteLine($"Current directory: '{Directory.GetCurrentDirectory()}'");
        }

        [Fact]
        public async Task Handle_ShouldDeletegateToPackageServiceAsync()
        {
            // Act
            await _sut.Handle(_command, default);

            // Assert
            _packageService.Verify(x => x.PackAsync(It.IsAny<PackCommand>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithRelativePath_ShouldGetFullPathAsync()
        {
            // Arrange
            var testPlcProject = _testProject.GetManagedPlcProjects().First();
            _command.Path = Path.GetRelativePath(Directory.GetCurrentDirectory(), testPlcProject.AbsolutePath);
            Path.IsPathFullyQualified(_command.Path).Should().BeFalse();
            _output.WriteLine(_command.Path);

            // Act
            await _sut.Handle(_command, default);
            _output.WriteLine(_command.Path);

            // Assert
            Path.IsPathFullyQualified(_command.Path).Should().BeTrue();
        }

        [Fact]
        public async Task Handle_WithRelativeSolutionPath_ShouldGetFullPathAsync()
        {
            // Arrange
            _command.Solution = Path.GetRelativePath(Directory.GetCurrentDirectory(), _testProject.SolutionPath);
            Path.IsPathFullyQualified(_command.Solution).Should().BeFalse();
            _output.WriteLine(_command.Solution);

            // Act
            await _sut.Handle(_command, default);
            _output.WriteLine(_command.Solution);

            // Assert
            Path.IsPathFullyQualified(_command.Solution).Should().BeTrue();
        }

        [Fact]
        public async Task Handle_WithNoOutputDirectory_ShouldSetToCurrentDirectoryAsync()
        {
            // Arrange
            _command.OutputDirectory = string.Empty;

            // Act
            await _sut.Handle(_command, default);

            // Assert
            _command.OutputDirectory.Should().Be(Directory.GetCurrentDirectory());
        }

        [Fact]
        public async Task Handle_WithRelativeOutputDirectoryPath_ShouldGetFullPathAsync()
        {
            // Arrange
            _command.OutputDirectory = Path.GetRelativePath(Directory.GetCurrentDirectory(), _testProject.RootPath);
            Path.IsPathFullyQualified(_command.OutputDirectory).Should().BeFalse();
            _output.WriteLine(_command.OutputDirectory);

            // Act
            await _sut.Handle(_command, default);
            _output.WriteLine(_command.OutputDirectory);

            // Assert
            Path.IsPathFullyQualified(_command.OutputDirectory).Should().BeTrue();
        }
    }
}
