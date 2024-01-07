// This file is licensed to you under MIT license.

using FluentValidation;
using Microsoft.Extensions.Logging;
using TwinGet.Core.Packaging;

namespace TwinGet.Core.Commands;

public class PackCommandValidator : AbstractValidator<PackCommand>
{
    private readonly ILogger? _logger;

    public PackCommandValidator(ILogger? logger)
    {
        _logger = logger;

        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(p => p.Path)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ErrorStrings.InputFileNotSpecified)
            .NotNull()
            .WithMessage(ErrorStrings.InputFileNotSpecified)
            .Must(File.Exists)
            .WithMessage(p => string.Format(ErrorStrings.InputFileNotFound, p.Path));

        RuleFor(p => p.Solution)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithSeverity(Severity.Warning)
            .WithMessage(SuggestionStrings.SpecifySolutionFileForBetterPerformance)
            .NotEmpty()
            .WithSeverity(Severity.Warning)
            .WithMessage(SuggestionStrings.SpecifySolutionFileForBetterPerformance)
            .Must(
                (solution) =>
                {
                    if (string.IsNullOrEmpty(solution))
                        return true;

                    return File.Exists(solution);
                }
            )
            .WithMessage(p => string.Format(ErrorStrings.SolutionFileNotFound, p.Solution));
    }
}
