// This file is licensed to you under MIT license.

using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using TwinGet.Core.Packaging;

namespace TwinGet.Core.Commands
{
    public class PackCommandHander(IValidator<PackCommand> validator, IPackageService packageService) : IRequestHandler<PackCommand, bool>
    {
        private readonly IValidator<PackCommand> _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        private readonly IPackageService _packageService = packageService ?? throw new ArgumentNullException(nameof(packageService));

        public async Task<bool> Handle(PackCommand request, CancellationToken cancellationToken)
        {
            FluentValidation.Results.ValidationResult result = await _validator.ValidateAsync(request, cancellationToken);

            if (!result.IsValid)
            {
                throw new PackagingException(result.Errors.ToList());
            }

            request.Logger?.LogInformation(PackagingStrings.AttemptingToBuildPackage, Path.GetFileName(request.Path));

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

            return await _packageService.PackAsync(request);
        }
    }
}
