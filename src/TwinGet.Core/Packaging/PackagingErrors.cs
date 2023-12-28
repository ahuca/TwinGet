// This file is licensed to you under MIT license.

using static NuGet.Configuration.NuGetConstants;
using static TwinGet.TwincatInterface.TwincatConstants;

namespace TwinGet.Core.Packaging
{
    public static class PackagingErrors
    {
        public static readonly string InputFileNotSpecified =
            $"Specify a {PlcProjectExtension} or a {ManifestExtension} file to use";
        public static readonly string InputFileNotSupported = InputFileNotSpecified;
        public const string InputFileNotFound = "Input file not found at '{0}'";
        public const string SolutionFileNotFound = "Solution file not found at '{0}'";
        public const string SpecifiedInputFileDoesNotBelongToSolution =
            "The specified input file '{0}' does not belong to the solution '{1}'";
        public const string FailedToResolveSolutionFile =
            "Failed to automatically resolve the parent solution file for the provided project file '{Path}'";
        public const string FailedToSavePlcLibrary =
            "Failed to save the PLC project '{Path}' as library.\n{ExceptionMessage}";
    }
}
