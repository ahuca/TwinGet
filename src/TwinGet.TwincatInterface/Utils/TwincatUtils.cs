// This file is licensed to you under MIT license.

using Microsoft.Build.Construction;
using TCatSysManagerLib;
using TwinGet.TwincatInterface.Dto;
using TwinGet.TwincatInterface.Exceptions;
using ITcSmTreeItemAlias = TCatSysManagerLib.ITcSmTreeItem9;
using ITcSysManagerAlias = TCatSysManagerLib.ITcSysManager15;
using TwingetProjectElement = TwinGet.TwincatInterface.Dto.ProjectElement;

namespace TwinGet.TwincatInterface.Utils
{
    public static class TwincatUtils
    {
        public static ITcPlcIECProject3? LookUpPlcProject(
            this ITcSysManagerAlias systemManager,
            string name
        )
        {
            ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));

            var treeItem = (ITcSmTreeItemAlias)
                systemManager.LookupTreeItem($"TIPC^{name}^{name} Project");
            var plcProject =
                (ITcPlcIECProject3)treeItem
                ?? throw new CouldNotLookUpPlcProject(
                    $"Could not look up any PLC project with the provided name",
                    name
                );

            return plcProject;
        }

        /// <summary>
        /// Wraps the <see cref="TwinGet.Utils.Xml.XmlSerializer.TryDeserializeXmlFile"/> and throw if the deserialized object is null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <exception cref="InvalidProjectFileFormat"></exception>
        public static T DeserializeXmlFileToProjectData<T>(string filePath)
            where T : ITwincatProjectData
        {
            T? projectData =
                TwinGet.Utils.Xml.XmlSerializer.TryDeserializeXmlFile<T>(filePath)
                ?? throw new InvalidProjectFileFormat(
                    ErrorStrings.InvalidProjectFileFormat,
                    filePath
                );

            return projectData;
        }

        /// <summary>
        /// Wraps the <see cref="TwinGet.Utils.Xml.XmlSerializer.TryDeserializeXmlFile"/> and throw if the deserialized object is null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <exception cref="InvalidProjectFileFormat"></exception>
        public static async Task<T> DeserializeXmlFileToProjectDataAsync<T>(string filePath)
            where T : ITwincatProjectData
        {
            T? projectData =
                await TwinGet.Utils.Xml.XmlSerializer.TryDeserializeXmlFileAsync<T>(filePath)
                ?? throw new InvalidProjectFileFormat(
                    ErrorStrings.InvalidProjectFileFormat,
                    filePath
                );

            return projectData;
        }

        /// <summary>
        /// Try to find the TwinCAT project that the given PLC project belongs to.
        /// </summary>
        /// <param name="plcProjectPath">The path to the <c>.plcproj</c> file.</param>
        /// <param name="upwardDepth">The upward depth of parents to search.</param>
        /// <returns>The absolute path to the TwinCAT project file if successfully found, otherwise <see cref="string.Empty"/>.</returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static string GetParentTwincatProjectFile(string plcProjectPath, int upwardDepth = 5)
        {
            ArgumentException.ThrowIfNullOrEmpty(plcProjectPath);
            PlcProjectFileHelper helper = new(plcProjectPath);
            return helper.GetParentTwincatProjectAsync(upwardDepth).Result;
        }

        /// <summary>
        /// Try to find the TwinCAT project that the given PLC project belongs to.
        /// </summary>
        /// <param name="plcProjectPath">The path to the <c>.plcproj</c> file.</param>
        /// <param name="upwardDepth">The upward depth of parents to search.</param>
        /// <returns>The absolute path to the TwinCAT project file if successfully found, otherwise <see cref="string.Empty"/>.</returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static async Task<string> GetParentTwincatProjectFileAsync(
            string plcProjectPath,
            int upwardDepth = 5
        )
        {
            ArgumentException.ThrowIfNullOrEmpty(plcProjectPath);
            PlcProjectFileHelper helper = new(plcProjectPath);
            return await helper.GetParentTwincatProjectAsync(upwardDepth);
        }

        /// <summary>
        /// Try to find the solution file that the given TwinCAT project belongs to.
        /// </summary>
        /// <param name="twincatProjectPath">The path to the TwinCAT project file.</param>
        /// <param name="upwardDepth">The upward depth of parents to search.</param>
        /// <returns>The absolute path to the solution file if successfully found, otherwise <see cref="string.Empty"/>.</returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static string GetParentSolutionFile(string twincatProjectPath, int upwardDepth = 5)
        {
            ArgumentException.ThrowIfNullOrEmpty(twincatProjectPath, nameof(twincatProjectPath));

            if (!File.Exists(twincatProjectPath))
            {
                throw new FileNotFoundException(
                    $"Provided plc project file path does not exists.",
                    twincatProjectPath
                );
            }

            // We first try to parse the TwinCAT project file and throw if necessary.
            TcSmProjectData? tcSmProjectData = DeserializeXmlFileToProjectData<TcSmProjectData>(
                twincatProjectPath
            );

            /// We recursively look up solution files in parent folders with a depth of <param name="upwardDepth"/param>.
            string parent = twincatProjectPath;
            for (int i = 0; i < upwardDepth - 1; i++)
            {
                parent = Directory.GetParent(parent)?.FullName ?? string.Empty;

                if (string.IsNullOrEmpty(parent))
                {
                    break;
                }

                var solutionFileCandidates = Directory.EnumerateFiles(
                    parent,
                    $"*{TwincatConstants.SolutionExtension}"
                );

                // We process each solution file we found.
                foreach (string solutionCandidate in solutionFileCandidates)
                {
                    // We should parse the solution file rather than simply reading a raw text file and string-match the GUID.
                    SolutionFile? solution = SolutionFile.Parse(solutionCandidate);

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

            return "";
        }

        /// <summary>
        /// Try to find the solution file that the given TwinCAT project belongs to.
        /// </summary>
        /// <param name="twincatProjectPath">The path to the TwinCAT project file.</param>
        /// <param name="upwardDepth">The upward depth of parents to search.</param>
        /// <returns>The absolute path to the solution file if successfully found, otherwise <see cref="string.Empty"/>.</returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static async Task<string> GetParentSolutionFileAsync(
            string twincatProjectPath,
            int upwardDepth = 5
        )
        {
            ArgumentException.ThrowIfNullOrEmpty(twincatProjectPath, nameof(twincatProjectPath));

            if (!File.Exists(twincatProjectPath))
            {
                throw new FileNotFoundException(
                    $"Provided plc project file path does not exists.",
                    twincatProjectPath
                );
            }

            // We first try to parse the TwinCAT project file and throw if necessary.
            //TcSmProjectData? tcSmProjectData = DeserializeXmlFileToProjectData<TcSmProjectData>(twincatProjectPath);
            Task<string> getTcProjectGuidTask = new TcSmProjectFileHelper(
                twincatProjectPath
            ).GetProjectGuidAsync();

            /// We recursively look up solution files in parent folders with a depth of <param name="upwardDepth"/param>.
            string parent = twincatProjectPath;
            for (int i = 0; i < upwardDepth - 1; i++)
            {
                parent = Directory.GetParent(parent)?.FullName ?? string.Empty;

                if (string.IsNullOrEmpty(parent))
                {
                    break;
                }

                var solutionFileCandidates = Directory.EnumerateFiles(
                    parent,
                    $"*{TwincatConstants.SolutionExtension}",
                    SearchOption.TopDirectoryOnly
                );

                // We process each solution file we found.
                foreach (string solutionCandidate in solutionFileCandidates)
                {
                    // We should parse the solution file rather than simply reading a raw text file and string-match the GUID.
                    SolutionFile? solution = SolutionFile.Parse(solutionCandidate);

                    if (solution is null)
                    {
                        continue;
                    }

                    var tcProjectGuid = await getTcProjectGuidTask;
                    // If that solution contains our GUID, we return it.
                    if (solution.ProjectsByGuid.ContainsKey(tcProjectGuid))
                    {
                        return solutionCandidate;
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Verify if the PLC project belongs to the solution.
        /// </summary>
        /// <param name="plcProjectPath">The path to the PLC project file.</param>
        /// <param name="solutionPath">The path to the solution file.</param>
        /// <returns>True if the PLC project belong to the solution, meaning the GUID and the path information match. Otherwise false.</returns>
        public static bool PlcProjectBelongToSolution(string plcProjectPath, string solutionPath)
        {
            ArgumentException.ThrowIfNullOrEmpty(plcProjectPath, nameof(plcProjectPath));
            ArgumentException.ThrowIfNullOrEmpty(solutionPath, nameof(solutionPath));

            PlcProjectData plcProjectData = DeserializeXmlFileToProjectData<PlcProjectData>(
                plcProjectPath
            );

            // We should parse the solution file to make sure it is valid.
            var solutionFile = SolutionFile.Parse(Path.GetFullPath(solutionPath));

            // We process each project in solution.
            foreach (ProjectInSolution? project in solutionFile.ProjectsInOrder)
            {
                // We skip any project that does not have TwinCAT project file extension.
                if (!IsTwincatProjectFileExtension(project.RelativePath))
                {
                    continue;
                }

                // We parse the TwinCAT project file.
                TcSmProjectData? tcProject = null;
                try
                {
                    tcProject = DeserializeXmlFileToProjectData<TcSmProjectData>(
                        project.AbsolutePath
                    );

                    static bool doesNotHavePlcProject(TcSmProjectData tcProject)
                    {
                        return tcProject.Project.Plc is null
                            || tcProject.Project.Plc?.Projects is null
                            || tcProject.Project.Plc?.Projects.Count == 0;
                    }

                    if (tcProject is null || doesNotHavePlcProject(tcProject))
                    {
                        continue;
                    }
                }
                catch
                {
                    continue;
                }

                // We process through each PLC project the TwinCAT project contains.
                foreach (TwingetProjectElement plcProject in tcProject.Project.Plc?.Projects)
                {
                    string candidatePath = Path.Combine(
                        Path.GetDirectoryName(project.AbsolutePath) ?? "",
                        plcProject.PrjFilePath
                    );
                    PlcProjectData candidate = DeserializeXmlFileToProjectData<PlcProjectData>(
                        candidatePath
                    );

                    bool isCorrectGuid = plcProjectData.PropertyGroup.ProjectGuid.Equals(
                        candidate.PropertyGroup.ProjectGuid,
                        StringComparison.OrdinalIgnoreCase
                    );
                    bool isCorrectPath = candidatePath.Equals(
                        plcProjectPath,
                        StringComparison.OrdinalIgnoreCase
                    );

                    if (isCorrectGuid && isCorrectPath)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Verify that the specified file has a valid TwinCAT project file extension. See <see cref="TwincatConstants.TwincatProjectExtensions"/>.
        /// </summary>
        /// <param name="filePath">The file path to verify.</param>
        /// <returns>True if the specified file has a valid TwinCAT project file extension.</returns>
        public static bool IsTwincatProjectFileExtension(string filePath)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));
            string fileExtension = Path.GetExtension(filePath);

            return TwincatConstants.TwincatProjectExtensions.Contains(
                fileExtension,
                StringComparer.OrdinalIgnoreCase
            );
        }

        /// <summary>
        /// Verify that the specified file has a valid PLC project file extension. See <see cref="TwincatConstants.PlcProjectExtension"/>.
        /// </summary>
        /// <param name="filePath">The file path to verify.</param>
        /// <returns>True if the specified file has a valid PLC project file extension.</returns>
        public static bool IsPlcProjectFileExtension(string filePath)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));
            string fileExtension = Path.GetExtension(filePath);

            return TwincatConstants.PlcProjectExtension.Equals(
                fileExtension,
                StringComparison.OrdinalIgnoreCase
            );
        }

        public static bool IsManagedLibrary(IPlcProjectMetadata projectMetadata)
        {
            ArgumentNullException.ThrowIfNull(projectMetadata, nameof(projectMetadata));
            return !string.IsNullOrEmpty(projectMetadata.Company)
                && !string.IsNullOrEmpty(projectMetadata.Title)
                && !string.IsNullOrEmpty(projectMetadata.ProjectVersion);
        }
    }
}
