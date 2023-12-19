﻿// This file is licensed to you under MIT license.

using static NuGet.Configuration.NuGetConstants;
using static TwinGet.AutomationInterface.TwincatConstants;

namespace TwinGet.Core.Packaging
{
    public static class PackagingErrors
    {
        public static readonly string InputFileNotSpecified = $"Specify a {PlcProjectExtension} or a {ManifestExtension} file to use";
        public const string InputFileNotFound = "Input file not found at {0}";
    }
}