// This file is licensed to you under MIT license.

using static NuGet.Configuration.NuGetConstants;
using static TwinGet.TwincatInterface.TwincatConstants;

namespace TwinGet.Core.Packaging;

public static class ErrorStrings
{
    public static readonly string InputFileNotSpecified =
        $"Specify a {PlcProjectExtension} or a {ManifestExtension} file to use.";
    public const string InputFileNotFound = "Input file not found at '{0}'.";
    public const string SolutionFileNotFound = "Solution file not found at '{0}',";
    public const string SpecifiedInputFileDoesNotBelongToSolution =
        "The specified input file '{PlcProject}' does not belong to the solution '{Solution}'.";
    public const string FailedToResolveSolutionFile =
        "Failed to automatically resolve the parent solution file for the provided project file '{Path}'.";
    public const string FailedToSavePlcLibrary =
        "Failed to save the PLC project '{Path}' as library.";
}
