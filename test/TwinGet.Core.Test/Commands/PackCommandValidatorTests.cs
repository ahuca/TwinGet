﻿// This file is licensed to you under MIT license.

using Test.Utils;
using TwinGet.Core.Commands;
using TwinGet.Core.Packaging;

namespace TwinGet.Core.Test.Commands
{
    public class PackCommandValidatorTests
    {
        private readonly PackCommand _command = new();
        private readonly PackCommandValidator _sut = new();
        private readonly TestProject _testProject = new();

        [Fact]
        public async Task ShouldHaveError_WhenPathIsNullAsync()
        {
            _command.Path = string.Empty;
            var result = await _sut.TestValidateAsync(_command);

            result.ShouldHaveValidationErrorFor(p => p.Path)
                .WithErrorMessage(PackagingErrors.InputFileNotSpecified);
        }

        [Fact]
        public async Task ShouldHaveError_WhenPathIsNotValidAsync()
        {
            _command.Path = "foo.bar";
            var result = await _sut.TestValidateAsync(_command);

            result.ShouldHaveValidationErrorFor(p => p.Path)
                .WithErrorMessage(PackagingErrors.InputFileNotSpecified);
        }

        [Fact]
        public async Task ShouldHaveError_WhenPathDoesNotExist()
        {
            _command.Path = "foo.plcproj";
            var result = await _sut.TestValidateAsync(_command);

            result.ShouldHaveValidationErrorFor(p => p.Path)
                .WithErrorMessage(string.Format(PackagingErrors.InputFileNotFound, _command.Path));
        }

        [Fact]
        public async Task ShouldHaveError_WhenPlcProjectDoesNotBelongToSolution()
        {
            using TestProject testProject2 = new();

            _command.Path = _testProject.GetPlcProjects().First().AbsolutePath;
            _command.Solution = testProject2.SolutionPath;

            var result = await _sut.TestValidateAsync(_command);
            var expectedMsg = string.Format(
                PackagingErrors.SpecifiedInputFileDoesNotBelongToSolution,
                _command.Path,
                _command.Solution);

            result.ShouldHaveValidationErrorFor(p => p.Solution)
                .WithErrorMessage(expectedMsg);
        }
    }
}
