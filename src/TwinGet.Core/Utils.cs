// This file is licensed to you under MIT license.

using TwinGet.TwincatInterface.Utils;
using static NuGet.Configuration.NuGetConstants;

namespace TwinGet.Core;

public static class Utils
{
    /// <summary>
    /// Verify that the given file has a nuspec extension.
    /// </summary>
    /// <param name="path">The file path to verify.</param>
    /// <returns>True if the given path has a nuspec extension, otherwise false.</returns>
    public static bool IsNuspecExtension(string path)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(path));

        return path.EndsWith(ManifestExtension, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Delegate to <see cref="TwincatUtils.PlcProjectBelongToSolution"/>.
    /// </summary>
    public static bool PlcProjectBelongToSolution(string plcProjectPath, string solutionPath)
    {
        return TwincatUtils.PlcProjectBelongToSolution(plcProjectPath, solutionPath);
    }

    public static bool IsPlcProjectFileExtension(string path)
    {
        return TwincatUtils.IsPlcProjectFileExtension(path);
    }
}
