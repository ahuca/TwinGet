// This file is licensed to you under MIT license.

using System.Reflection.Metadata.Ecma335;
using System.Xml;
using TwinGet.TwincatInterface.Dto;
using TwinGet.TwincatInterface.Exceptions;
using TwinGet.Utils.Xml;

namespace TwinGet.TwincatInterface.Utils
{
    public class TcSmProjectFileHelper
    {
        private readonly Lazy<TcSmProjectData> _projectData;
        private readonly Lazy<List<PlcProjectData>> _plcProjects;

        public IReadOnlyList<PlcProjectData> PlcProjects
        {
            get => _plcProjects.Value;
        }
        public string Directory { get; }
        public string AbsolutePath { get; }

        public TcSmProjectFileHelper(string filePath)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("TwinCAT project file not found.", filePath);
            }

            AbsolutePath = Path.GetFullPath(filePath);
            Directory = Path.GetDirectoryName(filePath) ?? string.Empty;

            _projectData = new Lazy<TcSmProjectData>(
                () =>
                    XmlSerializer.TryDeserializeXmlFile<TcSmProjectData>(filePath)
                    ?? throw new InvalidProjectFileFormat(
                        ErrorStrings.InvalidProjectFileFormat,
                        filePath
                    )
            );

            _plcProjects = new Lazy<List<PlcProjectData>>(() => GetPlcProjects().ToList());
        }

        private IEnumerable<PlcProjectData> GetPlcProjects()
        {
            if (DoesNotHavePlcProject())
            {
                yield break;
            }

            foreach (var plcProject in _projectData.Value.Project.Plc.Projects)
            {
                var result = XmlSerializer.TryDeserializeXmlFile<PlcProjectData>(
                    Path.GetFullPath(plcProject.PrjFilePath, Directory)
                );

                if (result is not null)
                {
                    yield return result;
                }
                else
                {
                    continue;
                }
            }
        }

        public async Task<string> GetProjectGuidAsync()
        {
            using var reader = XmlReader.Create(
                AbsolutePath,
                new XmlReaderSettings() { Async = true }
            );

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

        public bool DoesNotHavePlcProject()
        {
            return _projectData.Value.Project.Plc is null
                || _projectData.Value.Project.Plc?.Projects is null
                || _projectData.Value.Project.Plc?.Projects.Count == 0;
        }

        /// <summary>
        /// Verify if the given PLC project belong to this TwinCAT project.
        /// </summary>
        /// <param name="path">The path to the PLC project.</param>
        /// <returns></returns>
        public async Task<bool> HasPlcProjectAsync(string path)
        {
            ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));
            if (!Path.GetExtension(path).Equals(TwincatConstants.PlcProjectExtension))
            {
                throw new ArgumentException(
                    "The extension of the provided file is invalid.",
                    nameof(path)
                );
            }
            if (DoesNotHavePlcProject())
            {
                return false;
            }

            static bool stringsEqual(string x1, string x2) =>
                x1.Equals(x2, StringComparison.OrdinalIgnoreCase);

            PlcProjectFileHelper helper = new(path);
            string targetGuid = await helper.GetProjectGuidAsync();

            var candidate = _projectData.Value.Project.Plc.Projects.Where(
                p =>
                    stringsEqual(p.GUID, targetGuid)
                    && stringsEqual(path, Path.GetFullPath(p.PrjFilePath, Directory))
            );

            return candidate.Count() == 1;
        }
    }
}
