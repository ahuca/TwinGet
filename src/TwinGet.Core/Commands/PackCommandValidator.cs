// This file is licensed to you under MIT license.

using FluentValidation;
using TwinGet.Core.Packaging;

namespace TwinGet.Core.Commands;

public class PackCommandValidator : AbstractValidator<PackCommand>
{
    public PackCommandValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(p => p.Path)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(PackagingErrors.InputFileNotSpecified)
            .NotNull()
            .WithMessage(PackagingErrors.InputFileNotSpecified)
            .Must(File.Exists)
            .WithMessage(p => string.Format(PackagingErrors.InputFileNotFound, p.Path))
            .Must(Packaging.Utils.IsSupportedFileType)
            .WithMessage(PackagingErrors.InputFileNotSupported);

        RuleFor(p => p.Solution)
            .Cascade(CascadeMode.Stop)
            .Must(
                (solution) =>
                {
                    if (string.IsNullOrEmpty(solution))
                        return true;

                    return File.Exists(solution);
                }
            )
            .WithMessage(p => string.Format(PackagingErrors.SolutionFileNotFound, p.Solution))
            .Must(
                (packCommand, _) =>
                {
                    return VerifyPlcProjectRelationWithSolution(
                        packCommand.Path,
                        packCommand.Solution
                    );
                }
            )
            .WithMessage(
                p =>
                    string.Format(
                        PackagingErrors.SpecifiedInputFileDoesNotBelongToSolution,
                        p.Path,
                        p.Solution
                    )
            );
    }

    /// <summary>
    /// This method verify the relation between the given PLC project file and the solution file.
    /// </summary>
    /// <param name="plcProjectPath"></param>
    /// <param name="solutionPath"></param>
    /// <returns>True if no solution is provided (empty or null), or the PLC project is verified to belong the solution. False otherwise.</returns>
    private static bool VerifyPlcProjectRelationWithSolution(
        string? plcProjectPath,
        string? solutionPath
    )
    {
        // Solution file is optional, so if it's not provided we pass this validation.
        if (string.IsNullOrEmpty(solutionPath))
        {
            return true;
        }

        try
        {
            return Packaging.Utils.PlcProjectBelongToSolution(plcProjectPath, solutionPath);
        }
        catch
        {
            return false;
        }
    }
}
