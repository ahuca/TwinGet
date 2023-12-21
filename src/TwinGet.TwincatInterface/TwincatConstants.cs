// This file is licensed to you under MIT license.

namespace TwinGet.TwincatInterface
{
    public static class TwincatConstants
    {
        public static readonly IReadOnlyList<string> SupportedProgIds = new List<string> {
            "TcXaeShell.DTE.15.0",
            "VisualStudio.DTE.12.0",
            "VisualStudio.DTE.14.0",
            "VisualStudio.DTE.15.0"
        };

        public const string TwincatXaeDownloadUrl = @"https://www.beckhoff.com/en-en/support/download-finder/search-result/?search=eXtended%20Automation%20Engineering%20%28XAE%29";
        public const string TwincatXaeProjectKind = "{B1E792BE-AA5F-4E3C-8C82-674BF9C0715B}";
        public const string TwincatPlcProjectKind = TwincatXaeProjectKind;
        public const int ProjectItemStartingIndex = 1;
        public const string SolutionExtension = ".sln";
        public const string TwincatXaeProjectExtension = ".tsproj";
        public const string TwincatPlcProjectExtension = ".tspproj";
        public const string PlcProjectExtension = ".plcproj";
        public static readonly IReadOnlyList<string> TwincatProjectExtensions = new List<string>
        {
            TwincatXaeProjectExtension,
            TwincatPlcProjectExtension
        };
        public const string TwincatPlcLibraryExtension = ".library";
    }
}
