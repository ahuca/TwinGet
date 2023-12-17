// This file is licensed to you under MIT license.

using TCatSysManagerLib;
using TwinGet.AutomationInterface.Exceptions;
using TwinGet.AutomationInterface.ProjectFileDeserialization;
using TwinGet.AutomationInterface.Utils;

namespace TwinGet.AutomationInterface
{
    public class PlcProject : IPlcProject
    {
        private readonly ITcPlcIECProject3 _plcProject;
        private readonly PlcProjectData _plcProjectFile;

        public string Name { get => _plcProjectFile.PropertyGroup.Name; }
        public string? Company { get => _plcProjectFile.PropertyGroup.Company; }
        public string? Title { get => _plcProjectFile.PropertyGroup.Title; }
        public string? ProjectVersion { get => _plcProjectFile.PropertyGroup.ProjectVersion; }
        public bool IsManagedLibrary;

        public string FilePath { get; }

        public PlcProject(ITcSmTreeItem treeItem, string filePath) : this(filePath)
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
        protected PlcProject(string filePath)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));

            PlcProjectData plcProjectFile = TwincatUtils.DeserializeXmlFileToProjectData<PlcProjectData>(filePath);

            _plcProjectFile = plcProjectFile;

            FilePath = filePath;
        }

        public void PlcOpenExport(string bstrFile, string bstrSelection) => _plcProject.PlcOpenExport(bstrFile, bstrSelection);
        public void PlcOpenImport(string bstrFile, int options = 0, string bstrSelection = "", bool bSubTree = true) => _plcProject.PlcOpenImport(bstrFile, options, bstrSelection, bSubTree);
        public void SaveAsLibrary(string bstrLibraryPath, bool binstall = false) => _plcProject.SaveAsLibrary(bstrLibraryPath, binstall);
        public bool CheckAllObjects() => _plcProject.CheckAllObjects();
        public void RunStaticAnalysis(bool bCheckAll = true) => _plcProject.RunStaticAnalysis(bCheckAll);
    }
}
