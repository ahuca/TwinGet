// This file is licensed to you under MIT license.

using TCatSysManagerLib;
using TwinGet.AutomationInterface.Exceptions;
using TwinGet.AutomationInterface.ProjectFileDeserialization;
using ITcSmTreeItemAlias = TCatSysManagerLib.ITcSmTreeItem9;
using ITcSysManagerAlias = TCatSysManagerLib.ITcSysManager15;

namespace TwinGet.AutomationInterface.Utils
{
    internal static class TwincatUtils
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
        /// Try to find the TwinCAT project that the given PLC project belong to.
        /// </summary>
        /// <param name="plcProjectPath">The path to the <c>.plcproj</c> file.</param>
        /// <param name="upwardDepth"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static string GetParentTwincatProject(string plcProjectPath, int upwardDepth = 5)
        {
            ArgumentException.ThrowIfNullOrEmpty(plcProjectPath, nameof(plcProjectPath));

            if (!File.Exists(plcProjectPath))
            {
                throw new FileNotFoundException($"Provided plc project file path does not exists.", plcProjectPath);
            }

            // We first try to parse the plcproj file and throw if necessary.
            PlcProjectData? plcProjectFile = DeserializeXmlFileToProjectData<PlcProjectData>(plcProjectPath);

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
                    foreach (ProjectElement plcProjectCandidate in tcSmProject.Project.Plc.Projects)
                    {
                        // Using GUID, if we find ourselves in the TwinCAT project candidate, we return the candidate.
                        if (plcProjectCandidate.GUID.Equals(plcProjectFile.PropertyGroup.ProjectGuid, StringComparison.OrdinalIgnoreCase))
                        {
                            return tcCandidate;
                        }
                    }
                }
            }

            return "";
        }
    }
}
