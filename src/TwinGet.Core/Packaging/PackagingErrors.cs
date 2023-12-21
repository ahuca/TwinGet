// This file is licensed to you under MIT license.

using static NuGet.Configuration.NuGetConstants;
using static TwinGet.TwincatInterface.TwincatConstants;

namespace TwinGet.Core.Packaging
{
    public static class PackagingErrors
    {
        public static readonly string InputFileNotSpecified = $"Specify a {PlcProjectExtension} or a {ManifestExtension} file to use";
        public static readonly string InputFileNotSupported = InputFileNotSpecified;
        public const string InputFileNotFound = "Input file not found at {0}";
        public const string SpecifiedInputFileDoesNotBelongToSolution = "The specified input file {0} does not belong to the solution {1}";
    }
}
