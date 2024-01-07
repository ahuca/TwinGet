// This file is licensed to you under MIT license.

namespace TwinGet.TwincatInterface;

public static class PlcProjectMetadataExtensions
{
    public static bool IsManagedLibrary(this IPlcProjectMetadata metadata)
    {
        return !string.IsNullOrEmpty(metadata.Company)
            && !string.IsNullOrEmpty(metadata.Title)
            && !string.IsNullOrEmpty(metadata.ProjectVersion);
    }
}
