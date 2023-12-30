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

                    if (tcProject is null || !tcProject.HasProject())
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

        public static bool IsManagedLibrary(IPlcProjectMetadata projectMetadata) // TODO: make extension method for IPlcProjectMetadata instead of this
        {
            ArgumentNullException.ThrowIfNull(projectMetadata, nameof(projectMetadata));
            return !string.IsNullOrEmpty(projectMetadata.Company)
                && !string.IsNullOrEmpty(projectMetadata.Title)
                && !string.IsNullOrEmpty(projectMetadata.ProjectVersion);
        }
    }
}
