// This file is licensed to you under MIT license.

using System.Xml.Serialization;

namespace TwinGet.TwincatInterface.Dto.Test
{
    public class PlcProjectDataTests
    {
        private class TestData
        {
            public static IEnumerable<object[]> ValidXmlContents()
            {

                // Managed library, meaning has Company, Title, and ProjectVersion information.
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
                    "TwinGet.TestTwincatProject1.TestPlcProject1",
                    "0.1.0"
                };

                // Unmanaged library.
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
                        <SelectedLibraryCategories>
                          <Id xmlns="">{736e1fb9-7997-4a5d-a19a-cfcf55bcd1a4}</Id>
                        </SelectedLibraryCategories>
                      </PropertyGroup>
                    </Project>
                    """,
                    "TestPlcProject1",
                    null,
                    null,
                };
            }
        }

        [Theory]
        [MemberData(nameof(TestData.ValidXmlContents), MemberType = typeof(TestData))]
        public void Deserialize_FromValidXmlContent_ShouldSucceed(string xmlContent, string? projectName, string projectTitle, string projectVersion)
        {
            XmlSerializer serializer = new(typeof(PlcProjectData));

            using (StringReader reader = new(xmlContent))
            {
                PlcProjectData? project = serializer.Deserialize(reader) as PlcProjectData;

                project.Should().NotBeNull();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                project.PropertyGroup.Name.Should().Be(projectName);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                project.PropertyGroup.Title.Should().Be(projectTitle);
                project.PropertyGroup.ProjectVersion.Should().Be(projectVersion);
            }
        }
    }
}
