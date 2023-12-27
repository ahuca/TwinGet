// This file is licensed to you under MIT license.

using System.Xml.Serialization;
using TwinGet.TwincatInterface;
using TwinGet.TwincatInterface.Dto;
using TwinGet.TwincatInterface.Exceptions;
using TwinGet.TwincatInterface.Utils;

namespace Test.Utils
{
    internal class TestPlcProject : IPlcProjectMetadata
    {
        private readonly PlcProjectData _data;

        public string Name { get => _data.PropertyGroup.Name; }
        public string? Company { get => _data.PropertyGroup.Company; }
        public string Title { get => _data.PropertyGroup.Title; }
        public string? ProjectVersion { get => _data.PropertyGroup.ProjectVersion; }
        public string ProjectGuid { get => _data.PropertyGroup.ProjectGuid; }
        public string AbsolutePath { get; }
        public bool IsManagedLibrary { get => TwincatUtils.IsManagedLibrary(this); }

        public TestPlcProject(string path)
        {
            AbsolutePath = Path.GetFullPath(path);

            string xmlContent = File.ReadAllText(AbsolutePath);
            XmlSerializer serializer = new(typeof(PlcProjectData));

            using (StringReader reader = new(xmlContent))
            {
                _data = serializer.Deserialize(reader) as PlcProjectData ?? throw new InvalidProjectFileFormat("Failed to parse the PLC project file.", path);
            }
        }
    }
}
