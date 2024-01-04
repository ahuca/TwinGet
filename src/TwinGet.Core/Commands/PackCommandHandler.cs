// This file is licensed to you under MIT license.

using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using TwinGet.Core.Packaging;

namespace TwinGet.Core.Commands;

public class PackCommandHander(
    IValidator<PackCommand> validator,
    IPackStrategyFactory strategyFactory,
    ILogger? logger
) : IRequestHandler<PackCommand, bool>
{
    private readonly IValidator<PackCommand> _validator =
        validator ?? throw new ArgumentNullException(nameof(validator));
    private readonly IPackStrategyFactory _strategyFactory = strategyFactory;
    private readonly ILogger? _logger = logger;

    /// <summary>
    /// Handles <see cref="PackCommand"/>.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>True if successful, false otherwise.</returns>
    /// <exception cref="PackagingException"></exception>
    public async Task<bool> Handle(PackCommand request, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<PackCommand>(request);

        IPackStrategy strategy = _strategyFactory
            .CreateStrategy(request.Path)
            .DoCustomValidation(context);

        ArgumentNullException.ThrowIfNull(strategy, nameof(strategy));

        FluentValidation.Results.ValidationResult result = await _validator.ValidateAsync(
            context,
            cancellationToken
        );

        var errors = result.Errors.Where(e => e.Severity == Severity.Error);
        var warningsAndInfos = result.Errors.Where(
            e => e.Severity == Severity.Warning || e.Severity == Severity.Info
        );

        /// We throw when there are errors instead of checking <see cref="FluentValidation.Results.ValidationResult.IsValid"/>.
        /// See: https://github.com/FluentValidation/FluentValidation/issues/1519
        if (errors.Any())
        {
            throw new PackagingException(result.Errors.ToList());
        }

        // Log any errors with severity lower than Severity.Error
        foreach (var wi in warningsAndInfos)
        {
            if (wi.Severity == Severity.Warning)
            {
                request.Logger?.LogWarning(wi.ErrorMessage);
            }
            else if (wi.Severity == Severity.Info)
            {
                request.Logger?.LogWarning(wi.ErrorMessage);
            }
        }

        request.Logger?.LogInformation(
            OtherStrings.AttemptingToBuildPackage,
            Path.GetFileName(request.Path)
        );

        request.Path = Path.GetFullPath(request.Path);

        if (!string.IsNullOrEmpty(request.Solution))
        {
            request.Solution = Path.GetFullPath(request.Solution);
        }

        if (!string.IsNullOrEmpty(request.OutputDirectory))
        {
            request.OutputDirectory = Path.GetFullPath(request.OutputDirectory);
        }
        else
        {
            request.OutputDirectory = Directory.GetCurrentDirectory();
        }

        return await strategy.PackAsync(request);
    }
}
