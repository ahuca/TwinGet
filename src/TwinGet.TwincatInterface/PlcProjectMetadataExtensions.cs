// This file is licensed to you under MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Evaluation;

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
