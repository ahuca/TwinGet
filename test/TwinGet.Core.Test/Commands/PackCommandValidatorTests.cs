// This file is licensed to you under MIT license.

using Test.Utils;
using TwinGet.Core.Commands;
using TwinGet.Core.Packaging;
using TwinGet.TwincatInterface;

namespace TwinGet.Core.Test.Commands;

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

        result
            .ShouldHaveValidationErrorFor(p => p.Path)
            .WithErrorMessage(Core.Packaging.ErrorStrings.InputFileNotSpecified);
    }

    [Fact]
    public async Task ShouldHaveError_WhenPathIsNotValidAsync()
    {
        _command.Path = "foo.bar";
        var result = await _sut.TestValidateAsync(_command);

        result
            .ShouldHaveValidationErrorFor(p => p.Path)
            .WithErrorMessage(
                string.Format(Core.Packaging.ErrorStrings.InputFileNotFound, _command.Path)
            );
    }

    [Fact]
    public async Task ShouldHaveError_WhenPathDoesNotExistAsync()
    {
        _command.Path = "foo.plcproj";
        var result = await _sut.TestValidateAsync(_command);

        result
            .ShouldHaveValidationErrorFor(p => p.Path)
            .WithErrorMessage(
                string.Format(Core.Packaging.ErrorStrings.InputFileNotFound, _command.Path)
            );
    }

    [Fact]
    public async Task ShouldHaveError_WhenSolutionIsProvidedAndDoesNotExistAsync()
    {
        using TestProject testProject2 = new();

        _command.Path = _testProject.GetManagedPlcProjects().First().AbsolutePath;
        _command.Solution = $"foo{TwincatConstants.SolutionExtension}";

        var result = await _sut.TestValidateAsync(_command);
        var expectedMsg = string.Format(
            Core.Packaging.ErrorStrings.SolutionFileNotFound,
            _command.Solution
        );

        result.ShouldHaveValidationErrorFor(p => p.Solution).WithErrorMessage(expectedMsg);
    }

    [Fact]
    public async Task ShouldHaveError_WhenPlcProjectDoesNotBelongToSolutionAsync()
    {
        using TestProject testProject2 = new();

        _command.Path = _testProject.GetManagedPlcProjects().First().AbsolutePath;
        _command.Solution = testProject2.SolutionPath;

        var result = await _sut.TestValidateAsync(_command);
        var expectedMsg = string.Format(
            Core.Packaging.ErrorStrings.SpecifiedInputFileDoesNotBelongToSolution,
            _command.Path,
            _command.Solution
        );

        result.ShouldHaveValidationErrorFor(p => p.Solution).WithErrorMessage(expectedMsg);
    }

    [Fact]
    public async Task ShouldWarn_WhenNoSolutionWasProvided()
    {
        // Arrange
        _command.Path = _testProject.GetManagedPlcProjects().First().AbsolutePath;
        _command.Solution = string.Empty;

        // Act
        var result = await _sut.TestValidateAsync(_command);
        var actual = result.Errors.Where(e => e.Severity == FluentValidation.Severity.Warning);

        // Assert
        actual.Should().NotBeEmpty();
        actual
            .First()
            .ErrorMessage.Should()
            .Be(SuggestionStrings.SpecifySolutionFileForBetterPerformance);
    }
}
