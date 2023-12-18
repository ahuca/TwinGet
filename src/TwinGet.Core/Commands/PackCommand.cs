// This file is licensed to you under MIT license.

using MediatR;
using Microsoft.Extensions.Logging;

namespace TwinGet.Core.Commands;

public class PackCommand : IRequest<bool>, IPackCommand
{
    public ILogger Logger { get; set; }
    public string Path { get; set; }
    public string Solution { get; set; }
    public string OutputDirectory { get; set; }
}

public class PackCommandHander : IRequestHandler<PackCommand, bool>
{
    public async Task<bool> Handle(PackCommand request, CancellationToken cancellationToken)
    {
        request.Logger?.LogInformation("Package handle");
        return false;
    }
}
