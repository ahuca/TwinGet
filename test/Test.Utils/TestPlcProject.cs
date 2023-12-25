// This file is licensed to you under MIT license.

using System.Xml.Serialization;
using TwinGet.TwincatInterface.Dto;
using TwinGet.TwincatInterface.Exceptions;

namespace Test.Utils
{
    internal class TestPlcProject
    {
        private readonly PlcProjectData _data;

        public string Title { get => _data.PropertyGroup.Title; }
        public string Name { get => _data.PropertyGroup.Name; }
        public string GUID { get => _data.PropertyGroup.ProjectGuid; }
        public string AbsolutePath { get; }

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
