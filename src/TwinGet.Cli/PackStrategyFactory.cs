// This file is licensed to you under MIT license.

using Microsoft.Extensions.Logging;
using TwinGet.Core.Commands;
using TwinGet.Core.Packaging;

namespace TwinGet.Cli;

public class PackStrategyFactory(ILogger? logger) : IPackStrategyFactory
{
    private readonly ILogger? _logger = logger;
    private const string StrategyCreated = "'{Name}' pack strategy created.";

    public IPackStrategy CreateStrategy(string filePath)
    {
        ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));

        if (Core.Utils.IsPlcProjectFileExtension(filePath))
        {
            _logger?.LogDebug(StrategyCreated, nameof(PlcProjectPackStrategy));
            return new PlcProjectPackStrategy(_logger);
        }

        if (Core.Utils.IsNuspecExtension(filePath))
        {
            _logger?.LogDebug(StrategyCreated, nameof(NuspecPackStrategy));
            return new NuspecPackStrategy();
        }

        throw new NotSupportedException(
            $"No supported pack strategy for the specified file \"{filePath}\""
        );
    }
}
