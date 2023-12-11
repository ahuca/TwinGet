// This file is licensed to you under MIT license.

using System.Xml.Serialization;
using TwinGet.AutomationInterface.ProjectFileDeserialization;

namespace TwinGet.AutomationInterface.Test.ProjectFileDeserialization
{
    public class PlcProjectTests
    {
        private class TestData
        {
            public static IEnumerable<object[]> ValidXmlContents()
            {
                yield return new object[]
                {
                    """
                    <?xml version="1.0" encoding="utf-8"?>
                    <Project DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
                      <PropertyGroup>
                        <FileVersion>1.0.0.0</FileVersion>
                        <SchemaVersion>2.0</SchemaVersion>
                        <ProjectGuid>{f423d9f4-0ef7-4885-8ec0-9a403a78ec70}</ProjectGuid>
                        <SubObjectsSortedByName>True</SubObjectsSortedByName>
                        <DownloadApplicationInfo>true</DownloadApplicationInfo>
                        <WriteProductVersion>true</WriteProductVersion>
                        <GenerateTpy>false</GenerateTpy>
                        <Name>TestPlcProject1</Name>
                        <ProgramVersion>3.1.4024.0</ProgramVersion>
                        <Application>{da8cb013-01e2-4e76-aad8-c63cb197c3b9}</Application>
                        <TypeSystem>{65a0cb05-45a0-424f-9370-5321fe656f5f}</TypeSystem>
                        <Implicit_Task_Info>{d1e585ee-3f7a-4fb2-acd6-080b80c26dc3}</Implicit_Task_Info>
                        <Implicit_KindOfTask>{e3297877-f97b-44dc-8428-54f3df37edf6}</Implicit_KindOfTask>
                        <Implicit_Jitter_Distribution>{74d54b3b-a48f-4f65-8df7-855cc9cd89e2}</Implicit_Jitter_Distribution>
                        <LibraryReferences>{bd174e45-2719-4cbf-b50e-b235af23264e}</LibraryReferences>
                        <Company>TwinGet</Company>
                        <Released>false</Released>
                        <Title>TwinGet.TestTwincatProject1.TestPlcProject1</Title>
                        <ProjectVersion>0.1.0</ProjectVersion>
                        <LibraryCategories>
                          <LibraryCategory xmlns="">
                            <Id>{736e1fb9-7997-4a5d-a19a-cfcf55bcd1a4}</Id>
                            <Version>1.0.0.0</Version>
                            <DefaultName>TwinGet</DefaultName>
                          </LibraryCategory>
                        </LibraryCategories>
                        <SelectedLibraryCategories>
                          <Id xmlns="">{736e1fb9-7997-4a5d-a19a-cfcf55bcd1a4}</Id>
                        </SelectedLibraryCategories>
                      </PropertyGroup>
                    </Project>
                    """,
                    "TestPlcProject1",
                    "0.1.0"
                };
            }
        }

        [Theory]
        [MemberData(nameof(TestData.ValidXmlContents), MemberType = typeof(TestData))]
        public void Deserialize_FromValidXmlContent_ShouldSucceed(string xmlContent, string projectName, string projectVersion)
        {
            XmlSerializer serializer = new(typeof(TwinGet.AutomationInterface.ProjectFileDeserialization.PlcProject));

            using (StringReader reader = new(xmlContent))
            {
                TwinGet.AutomationInterface.ProjectFileDeserialization.PlcProject? project = serializer.Deserialize(reader) as TwinGet.AutomationInterface.ProjectFileDeserialization.PlcProject;

                project.Should().NotBeNull();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                project.PropertyGroup.Name.Should().Be(projectName);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                project.PropertyGroup.ProjectVersion.Should().Be(projectVersion);
            }
        }
    }
}
