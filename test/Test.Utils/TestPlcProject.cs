// This file is licensed to you under MIT license.

using System.Xml.Serialization;
using TwinGet.AutomationInterface.Dto;
using TwinGet.AutomationInterface.Exceptions;

namespace Test.Utils
{
    internal class TestPlcProject
    {
        public string Name { get; }
        public string AbsolutePath { get; }

        public TestPlcProject(string path)
        {
            AbsolutePath = Path.GetFullPath(path);

            string xmlContent = File.ReadAllText(AbsolutePath);
            XmlSerializer serializer = new(typeof(PlcProjectData));

            using (StringReader reader = new(xmlContent))
            {
                var plcProjectData = serializer.Deserialize(reader) as PlcProjectData;

                Name = plcProjectData?.PropertyGroup.Name ?? throw new InvalidProjectFileFormat("Failed to get the PLC project name.", path);
            }
        }
    }
}
