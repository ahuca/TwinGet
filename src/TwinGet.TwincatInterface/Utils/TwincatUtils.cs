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
        public static ITcPlcIECProject3? LookUpPlcProject(this ITcSysManagerAlias systemManager, string name)
        {
            ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));

            var treeItem = (ITcSmTreeItemAlias)systemManager.LookupTreeItem($"TIPC^{name}^{name} Project");
            var plcProject = (ITcPlcIECProject3)treeItem ?? throw new CouldNotLookUpPlcProject($"Could not look up any PLC project with the provided name", name);

            return plcProject;
        }

        /// <summary>
        /// Wraps the <see cref="TwinGet.Utils.Xml.XmlSerializer.TryDeserializeXmlFileTo"/> and throw if the deserialized object is null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <exception cref="InvalidProjectFileFormat"></exception>
        public static T DeserializeXmlFileToProjectData<T>(string filePath) where T : ITwincatProjectData
        {
            T? projectData = TwinGet.Utils.Xml.XmlSerializer.TryDeserializeXmlFileTo<T>(filePath) ?? throw new InvalidProjectFileFormat("The format of the TwinCAT project file is invalid.", filePath);

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
            ArgumentException.ThrowIfNullOrEmpty(plcProjectPath, nameof(plcProjectPath));

            if (!File.Exists(plcProjectPath))
            {
                throw new FileNotFoundException($"Provided plc project file path does not exists.", plcProjectPath);
            }

            // We first try to parse the plcproj file and throw if necessary.
            PlcProjectData? plcProjectData = DeserializeXmlFileToProjectData<PlcProjectData>(plcProjectPath);

            /// We recursively look up TwinCAT project files in parent folders with a depth of <param name="upwardDepth"/param>.
            string parent = plcProjectPath;
            for (int i = 0; i < upwardDepth - 1; i++)
            {
                parent = Directory.GetParent(parent)?.FullName ?? string.Empty;

                if (string.IsNullOrEmpty(parent))
                {
                    break;
                }

                string[] xaeCandidates = Directory.GetFiles(parent, $"*{TwincatConstants.TwincatXaeProjectExtension}");
                string[] plcCandidates = Directory.GetFiles(parent, $"*{TwincatConstants.TwincatPlcProjectExtension}");
                string[] twincatProjectCandidate = xaeCandidates.Concat(plcCandidates).ToArray();

                if (twincatProjectCandidate.Length == 0) { continue; }

                // We process each TwinCAT project file we found.
                foreach (string tcCandidate in twincatProjectCandidate)
                {
                    TcSmProjectData? tcSmProject = TwinGet.Utils.Xml.XmlSerializer.TryDeserializeXmlFileTo<TcSmProjectData>(tcCandidate);

                    if (tcSmProject is null) { continue; }

                    // We process each PLC project the TwinCAT project has.
                    foreach (TwingetProjectElement plcProjectCandidate in tcSmProject.Project.Plc.Projects)
                    {
                        // Using GUID, if we find ourselves in the TwinCAT project candidate, we return the candidate.
                        if (plcProjectCandidate.GUID.Equals(plcProjectData.PropertyGroup.ProjectGuid, StringComparison.OrdinalIgnoreCase))
                        {
                            return tcCandidate;
                        }
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
        public static string GetParentSolutionFile(string twincatProjectPath, int upwardDepth = 5)
        {
            ArgumentException.ThrowIfNullOrEmpty(twincatProjectPath, nameof(twincatProjectPath));

            if (!File.Exists(twincatProjectPath))
            {
                throw new FileNotFoundException($"Provided plc project file path does not exists.", twincatProjectPath);
            }

            // We first try to parse the TwinCAT project file and throw if necessary.
            TcSmProjectData? tcSmProjectData = DeserializeXmlFileToProjectData<TcSmProjectData>(twincatProjectPath);

            /// We recursively look up solution files in parent folders with a depth of <param name="upwardDepth"/param>.
            string parent = twincatProjectPath;
            for (int i = 0; i < upwardDepth - 1; i++)
            {
                parent = Directory.GetParent(parent)?.FullName ?? string.Empty;

                if (string.IsNullOrEmpty(parent))
                {
                    break;
                }

                string[] solutionFileCandidates = Directory.GetFiles(parent, $"*{TwincatConstants.SolutionExtension}");

                if (solutionFileCandidates.Length == 0) { continue; }

                // We process each solution file we found.
                foreach (string solutionCandidate in solutionFileCandidates)
                {
                    // We should parse the solution file rather than simply reading a raw text file and string-match the GUID.
                    SolutionFile? solution = SolutionFile.Parse(solutionCandidate);

                    if (solution is null) { continue; }

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
        /// Verify if the PLC project belongs to the solution.
        /// </summary>
        /// <param name="plcProjectPath">The path to the PLC project file.</param>
        /// <param name="solutionPath">The path to the solution file.</param>
        /// <returns></returns>
        public static bool PlcProjectBelongToSolution(string plcProjectPath, string solutionPath)
        {
            ArgumentException.ThrowIfNullOrEmpty(plcProjectPath, nameof(plcProjectPath));
            ArgumentException.ThrowIfNullOrEmpty(solutionPath, nameof(solutionPath));

            PlcProjectData plcProjectData = DeserializeXmlFileToProjectData<PlcProjectData>(plcProjectPath);

            // We should parse the solution file to make sure it is valid.
            var solutionFile = SolutionFile.Parse(solutionPath);

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
                    tcProject = DeserializeXmlFileToProjectData<TcSmProjectData>(project.AbsolutePath);
                    if (tcProject is null)
                    {
                        continue;
                    }
                }
                catch { continue; }

                // We process through each PLC project the TwinCAT project contains.
                foreach (TwingetProjectElement plcProject in tcProject.Project.Plc.Projects)
                {
                    string candidatePath = Path.Combine(Path.GetDirectoryName(project.AbsolutePath) ?? "", plcProject.PrjFilePath);
                    PlcProjectData candidate = DeserializeXmlFileToProjectData<PlcProjectData>(candidatePath);

                    if (plcProjectData.PropertyGroup.ProjectGuid.Equals(candidate.PropertyGroup.ProjectGuid, StringComparison.OrdinalIgnoreCase))
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

            return TwincatConstants.TwincatProjectExtensions.Contains(fileExtension, StringComparer.OrdinalIgnoreCase);
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

            return TwincatConstants.PlcProjectExtension.Equals(fileExtension, StringComparison.OrdinalIgnoreCase);
        }
    }
}
