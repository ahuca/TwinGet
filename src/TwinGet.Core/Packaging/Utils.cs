// This file is licensed to you under MIT license.

using TwinGet.AutomationInterface.Utils;
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

            return isNuspecExtension && isPlcProjectExtension;
        }
    }
}
