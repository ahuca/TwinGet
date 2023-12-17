// This file is licensed to you under MIT license.

using System.Xml.Serialization;
using TwinGet.AutomationInterface.Dto;
using TwinGet.AutomationInterface.Exceptions;

namespace TwinGet.AutomationInterface.Test.TestUtils
{
    internal class TestTwincatProject
    {
        private readonly List<TestPlcProject> _plcProjects = [];
        public string Name { get; }
        public string AbsolutePath { get; }
        public IReadOnlyList<TestPlcProject> PlcProjects { get => _plcProjects; }

        public TestTwincatProject(string name, string absolutePath)
        {
            Name = name;
            AbsolutePath = absolutePath;

            string xmlContent = File.ReadAllText(absolutePath);
            XmlSerializer serializer = new(typeof(TcSmProjectData));
            string? rootDir = Path.GetDirectoryName(absolutePath);

            using (StringReader reader = new(xmlContent))
            {
                TcSmProjectData tcSmProject = serializer.Deserialize(reader) as TcSmProjectData ?? throw new InvalidProjectFileFormat("The format of TwinCAT project file is invalid.", absolutePath);

                if (tcSmProject.Project.Plc?.Projects is not null)
                {
                    _plcProjects.AddRange(from ProjectElement project in tcSmProject.Project.Plc.Projects
                                          let plcProject = new TestPlcProject(Path.Join(rootDir, project.PrjFilePath))
                                          where plcProject is not null
                                          select plcProject);
                }
            }
        }
    }
}
