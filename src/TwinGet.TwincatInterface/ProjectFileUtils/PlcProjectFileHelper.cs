// This file is licensed to you under MIT license.

using System.Xml;
using Microsoft.Build.Construction;
using TwinGet.TwincatInterface.Dto;
using TwinGet.TwincatInterface.Exceptions;
using TwinGet.TwincatInterface.Utils;
using IODirectory = System.IO.Directory;
using IOPath = System.IO.Path;

namespace TwinGet.TwincatInterface.ProjectFileUtils;

public class PlcProjectFileHelper : IPlcProjectFileHelper
{
    public string Path { get; }

    public string Directory { get; }

    private PlcProjectFileHelper(string filePath)
    {
        ArgumentException.ThrowIfNullOrEmpty(filePath);

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("PLC project file not found.", filePath);
        }

        if (!TwincatUtils.IsPlcProjectFileExtension(filePath))
        {
            throw new TwincatInterfaceException(
                ExceptionStrings.InvalidPlcProjectExtension,
                ExceptionStrings.ProjectFilesHelpLink
            );
        }

        Path = IOPath.GetFullPath(filePath);
        Directory = IOPath.GetDirectoryName(filePath) ?? string.Empty;
    }

    public static PlcProjectFileHelper Create(string filePath)
    {
        return new PlcProjectFileHelper(filePath);
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
                && reader.Name.Equals("ProjectGuid", StringComparison.OrdinalIgnoreCase)
            )
            {
                return await reader.ReadInnerXmlAsync();
            }
            // End of PropertyGroup element found, quit early.
            else if (
                reader.NodeType == XmlNodeType.EndElement
                && reader.Name.Equals("PropertyGroup", StringComparison.OrdinalIgnoreCase)
            )
            {
                return string.Empty;
            }
        }

        return string.Empty;
    }

    public string GetParentTwincatFile(int upwardDepth = 5) => GetParentTwincatFileAsync().Result;

    public async Task<string> GetParentTwincatFileAsync(int upwardDepth = 5)
    {
        ArgumentException.ThrowIfNullOrEmpty(Path, nameof(Path));
        if (!TwincatUtils.IsPlcProjectFileExtension(Path))
        {
            return string.Empty;
        }

        if (!File.Exists(Path))
        {
            throw new FileNotFoundException(
                $"Provided plc project file path does not exists.",
                Path
            );
        }

        /// We recursively look up TwinCAT project files in parent folders with a depth of <param name="upwardDepth"/param>.
        string parentDir = Path;
        for (int i = 0; i < upwardDepth; i++)
        {
            parentDir = IODirectory.GetParent(parentDir)?.FullName ?? string.Empty;

            if (string.IsNullOrEmpty(parentDir))
            {
                // If we can't even get the first parent, we return.
                return string.Empty;
            }

            var candidates = IODirectory
                .EnumerateFiles(
                    parentDir,
                    $"*{TwincatConstants.TwincatProjectWildcardExtension}",
                    SearchOption.TopDirectoryOnly
                )
                .Where(TwincatUtils.IsTwincatProjectFileExtension);

            var candidateChecks = candidates.ToDictionary(x => x, IsParentTwincatProjectAsync);

            foreach (var (candidate, isParent) in candidateChecks)
            {
                if (await isParent)
                {
                    return candidate;
                }
            }
        }

        return string.Empty;
    }

    private async Task<bool> IsParentTwincatProjectAsync(string path)
    {
        ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));

        var getGuid = GuidAsync();
        var tcSmProject = TwincatUtils.DeserializeXmlFileToProjectData<TcSmProjectData>(path);

        if (!tcSmProject.HasProject())
        {
            return false;
        }

        if (tcSmProject.Project.Plc is null)
        {
            return false;
        }

        string myGuid = await getGuid;

        var myself = tcSmProject.Project.Plc.Projects.Where(
            x =>
                x.GUID.Equals(myGuid, StringComparison.OrdinalIgnoreCase)
                && IOPath
                    .GetFullPath(x.PrjFilePath, path)
                    .Equals(Path, StringComparison.OrdinalIgnoreCase)
        );

        return myself is not null;
    }

    public string GetParentSolutionFile(int upwardDepth = 5) =>
        GetParentSolutionFileAsync(upwardDepth).Result;

    public async Task<string> GetParentSolutionFileAsync(int upwardDepth = 5)
    {
        string tcParentFile = await GetParentTwincatFileAsync(upwardDepth);

        if (string.IsNullOrEmpty(tcParentFile))
        {
            return string.Empty;
        }

        if (!File.Exists(tcParentFile))
        {
            throw new FileNotFoundException(
                $"Provided plc project file path does not exists.",
                tcParentFile
            );
        }

        // We first try to parse the TwinCAT project file and throw if necessary.
        TcSmProjectData? tcSmProjectData =
            TwincatUtils.DeserializeXmlFileToProjectData<TcSmProjectData>(tcParentFile);

        /// We recursively look up solution files in parent folders with a depth of <param name="upwardDepth"/param>.
        string parent = tcParentFile;
        for (int i = 0; i < upwardDepth; i++)
        {
            parent = IODirectory.GetParent(parent)?.FullName ?? string.Empty;

            if (string.IsNullOrEmpty(parent))
            {
                break;
            }

            var solutionFileCandidates = IODirectory.EnumerateFiles(
                parent,
                $"*{TwincatConstants.SolutionExtension}"
            );

            // We process each solution file we found.
            foreach (string solutionCandidate in solutionFileCandidates)
            {
                // We should parse the solution file rather than simply reading a raw text file and string-match the GUID.
                var solution = SolutionFile.Parse(solutionCandidate);

                if (solution is null)
                {
                    continue;
                }

                // If that solution contains our GUID, we return it.
                if (solution.ProjectsByGuid.ContainsKey(tcSmProjectData.Project.ProjectGUID))
                {
                    return solutionCandidate;
                }
            }
        }

        return string.Empty;
    }
}
