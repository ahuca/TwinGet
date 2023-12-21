// This file is licensed to you under MIT license.

using FluentValidation;
using MediatR;
using TwinGet.Core.Packaging;

namespace TwinGet.Core.Commands
{
    public class PackCommandHander(IValidator<PackCommand> validator, IPackageService packageService) : IRequestHandler<PackCommand, bool>
    {
        private IValidator<PackCommand> _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        private readonly IPackageService _packageService = packageService ?? throw new ArgumentNullException(nameof(packageService));

        public async Task<bool> Handle(PackCommand request, CancellationToken cancellationToken)
        {
            FluentValidation.Results.ValidationResult result = await _validator.ValidateAsync(request, cancellationToken);

            if (!result.IsValid)
            {
                throw new PackagingException(result.Errors.ToList());
            }

            if (string.IsNullOrEmpty(request.OutputDirectory))
            {
                request.OutputDirectory = Directory.GetCurrentDirectory();
            }

            return await _packageService.PackAsync(request);
        }
    }
}
