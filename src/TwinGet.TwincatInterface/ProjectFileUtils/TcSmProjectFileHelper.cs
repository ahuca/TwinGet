// This file is licensed to you under MIT license.

using System.Xml;
using Microsoft.Build.Construction;
using TwinGet.TwincatInterface.Exceptions;
using TwinGet.TwincatInterface.Utils;
using IODirectory = System.IO.Directory;

namespace TwinGet.TwincatInterface.ProjectFileUtils
{
    public class TcSmProjectFileHelper : ITcSmProjectFileHelper
    {
        public string Directory { get; }
        public string Path { get; }

        private TcSmProjectFileHelper(string filePath)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("TwinCAT project file not found.", filePath);
            }

            if (!TwincatUtils.IsTwincatProjectFileExtension(filePath))
            {
                throw new TwincatInterfaceException(
                    ExceptionStrings.InvalidTwincatProjectExtension,
                    ExceptionStrings.ProjectFilesHelpLink
                );
            }

            Path = System.IO.Path.GetFullPath(filePath);
            Directory = System.IO.Path.GetDirectoryName(filePath) ?? string.Empty;
        }

        public static TcSmProjectFileHelper Create(string filePath)
        {
            return new TcSmProjectFileHelper(filePath);
        }

        public string Guid() => GuidAsync().Result;

        public async Task<string> GuidAsync()
        {
            using var reader = XmlReader.Create(Path, new XmlReaderSettings() { Async = true });

            while (!reader.EOF)
            {
                await reader.ReadAsync();

                if (
                    reader.NodeType == XmlNodeType.Element
                    && reader.Name.Equals("Project", StringComparison.OrdinalIgnoreCase)
                )
                {
                    if (!reader.HasAttributes)
                    {
                        continue;
                    }
                    string? guid = reader.GetAttribute("ProjectGUID");
                    if (string.IsNullOrEmpty(guid))
                    {
                        continue;
                    }
                    return guid;
                }
            }

            return string.Empty;
        }

        public string GetParentSolutionFile(int upwardDepth) =>
            GetParentSolutionFileAsync(upwardDepth).Result;

        public async Task<string> GetParentSolutionFileAsync(int upwardDepth)
        {
            /// We recursively look up solution files in parent folders with a depth of <param name="upwardDepth"/param>.
            string parent = Path;
            for (int i = 0; i < upwardDepth; i++)
            {
                parent = IODirectory.GetParent(parent)?.FullName ?? string.Empty;

                if (string.IsNullOrEmpty(parent))
                {
                    break;
                }

                var solutionFiles = IODirectory.EnumerateFiles(
                    parent,
                    $"*{TwincatConstants.SolutionExtension}"
                );

                // We process each solution file we found.
                foreach (string solutionFile in solutionFiles)
                {
                    // We should parse the solution file rather than simply reading a raw text file and string-match the GUID.
                    var solution = SolutionFile.Parse(solutionFile);

                    if (solution is null)
                    {
                        continue;
                    }

                    // If that solution contains our GUID, we return it.
                    string myGuid = await GuidAsync();
                    var candidate = solution
                        .ProjectsByGuid.Where(x => x.Value.ProjectGuid.Equals(myGuid))
                        .Select(x => x.Value)
                        .First();
                    if (candidate is null)
                    {
                        continue;
                    }

                    if (candidate.ProjectGuid.Equals(myGuid, StringComparison.OrdinalIgnoreCase))
                    {
                        return solutionFile;
                    }
                }
            }

            return string.Empty;
        }
    }
}
