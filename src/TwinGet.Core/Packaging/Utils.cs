// This file is licensed to you under MIT license.

using TwinGet.TwincatInterface.Utils;
using static NuGet.Configuration.NuGetConstants;

namespace TwinGet.Core.Packaging
{
    public static class Utils
    {
        public static bool IsSupportedFileType(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return false;

            bool isNuspecExtension = filePath.EndsWith(ManifestExtension, StringComparison.OrdinalIgnoreCase);
            bool isPlcProjectExtension = TwincatUtils.IsPlcProjectFileExtension(filePath);

            return isNuspecExtension || isPlcProjectExtension;
        }

        /// <summary>
        /// Delegate to <see cref="TwincatUtils.PlcProjectBelongToSolution"/>.
        /// </summary>
        public static bool PlcProjectBelongToSolution(string plcProjectPath, string solutionPath)
        {
            return TwincatUtils.PlcProjectBelongToSolution(plcProjectPath, solutionPath);
        }
    }
}
