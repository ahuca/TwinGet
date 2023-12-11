// This file is licensed to you under MIT license.

using System.Xml.Serialization;
using TCatSysManagerLib;
using TwinGet.AutomationInterface.Exceptions;

namespace TwinGet.AutomationInterface
{
    public class PlcProject : ITcPlcIECProject3
    {
        private readonly ITcPlcIECProject3 _plcProject;
        private readonly ProjectFileDeserialization.PlcProjectData _plcProjectFile;

        public string Name { get => _plcProjectFile.PropertyGroup.Name; }
        public string Path { get; }


        public PlcProject(ITcSmTreeItem treeItem, string path) : this(path)
        {
            try
            {
                _plcProject = (ITcPlcIECProject3)treeItem;
            }
            catch
            {
                throw new NotAPlcProject($"The provided tree item {treeItem.Name} is not a PLC project.");
            }
        }

        internal PlcProject(ITcPlcIECProject3 plcProject, string path) : this(path)
        {
            _plcProject = plcProject;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected PlcProject(string path)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));

            string xmlContent = File.ReadAllText(path);
            XmlSerializer serializer = new(typeof(ProjectFileDeserialization.PlcProjectData));

            using (StringReader reader = new(xmlContent))
            {
                ProjectFileDeserialization.PlcProjectData plcProjectFile = serializer.Deserialize(reader) as ProjectFileDeserialization.PlcProjectData ?? throw new InvalidProjectFileFormat("Could not deserialize the provided PLC project file.", path);
                _plcProjectFile = plcProjectFile;
            }

            Path = path;
        }

        public void PlcOpenExport(string bstrFile, string bstrSelection) => _plcProject.PlcOpenExport(bstrFile, bstrSelection);
        public void PlcOpenImport(string bstrFile, int options = 0, string bstrSelection = "", bool bSubTree = true) => _plcProject.PlcOpenImport(bstrFile, options, bstrSelection, bSubTree);
        public void SaveAsLibrary(string bstrLibraryPath, bool binstall = false) => _plcProject.SaveAsLibrary(bstrLibraryPath, binstall);
        public bool CheckAllObjects() => _plcProject.CheckAllObjects();
        public void RunStaticAnalysis(bool bCheckAll = true) => _plcProject.RunStaticAnalysis(bCheckAll);
    }
}
