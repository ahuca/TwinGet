// This file is licensed to you under MIT license.

using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using TwinGet.AutomationInterface.Utils;
using TwinGet.Core.Packaging;
using static NuGet.Configuration.NuGetConstants;

namespace TwinGet.Core.Commands;

public class PackCommand : IRequest<bool>, IPackCommand
{
    public ILogger Logger { get; set; }
    public string Path { get; set; }
    public string Solution { get; set; }
    public string OutputDirectory { get; set; }
}

public class PackCommandHander(IValidator<PackCommand> validator) : IRequestHandler<PackCommand, bool>
{
    private IValidator<PackCommand> _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    public enum PackFileType
    {
        None,
        PlcProject,
        Nuspec
    }

    public static PackFileType GetFileType(string filePath)
    {
        if (TwincatUtils.IsPlcProjectFileExtension(filePath))
        {
            return PackFileType.PlcProject;
        }
        else if (filePath.EndsWith(ManifestExtension, StringComparison.OrdinalIgnoreCase))
        {
            return PackFileType.Nuspec;
        }

        return PackFileType.None;
    }

    public async Task<bool> Handle(PackCommand request, CancellationToken cancellationToken)
    {
        PackFileType fileType = GetFileType(request.Path);

        FluentValidation.Results.ValidationResult result = await _validator.ValidateAsync(request, cancellationToken);

        if (!result.IsValid)
        {
            throw new PackagingException(result.Errors.ToList());
        }

        // TODO: continue
        if (!string.IsNullOrEmpty(request.Solution))
        {

        }

        string twincatProject = TwincatUtils.GetParentTwincatProjectFile(request.Path);
        if (string.IsNullOrEmpty(twincatProject))
        {
            throw new PackagingException(""); // TODO
        }

        request.Logger?.LogInformation("Package handle");
        return false;
    }
}
