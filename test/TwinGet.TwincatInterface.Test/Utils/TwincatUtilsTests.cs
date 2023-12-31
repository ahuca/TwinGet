// This file is licensed to you under MIT license.

using TwinGet.TwincatInterface.Dto;
using TwinGet.TwincatInterface.Utils;
using Xunit.Abstractions;

namespace TwinGet.TwincatInterface.Test.Utils;

public class TwincatUtilsTests(ITestOutputHelper output)
{
    private class TestData
    {
        public static IEnumerable<object[]> PlcProjectFiles()
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
                new Dictionary<string, string>
                {
                    { "Name", "TestPlcProject1" },
                    { "Title", "TwinGet.TestTwincatProject1.TestPlcProject1" },
                    { "Version", "0.1.0" },
                }
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
                new Dictionary<string, string>
                {
                    { "Name", "TestPlcProject1" },
                    { "Title", string.Empty },
                    { "Version", string.Empty },
                }
            };
        }

        public static IEnumerable<object[]> TwincatProjectFiles()
        {
            yield return new object[]
            {
                """
                <?xml version="1.0"?>
                <TcSmProject xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:noNamespaceSchemaLocation="http://www.beckhoff.com/schemas/2012/07/TcSmProject" TcSmVersion="1.0" TcVersion="3.1.4024.53">
                	<Project ProjectGUID="{AF0AA87D-6A50-4129-B38E-8931819C4FEB}" ShowHideConfigurations="#x3c7">
                		<System>
                			<Tasks>
                				<Task Id="3" Priority="20" CycleTime="100000" AmsPort="350" AdtTasks="true">
                					<Name>PlcTask</Name>
                				</Task>
                			</Tasks>
                		</System>
                		<Plc>
                			<Project GUID="{F423D9F4-0EF7-4885-8EC0-9A403A78EC70}" Name="TestPlcProject1" PrjFilePath="TestPlcProject1\TestPlcProject1.plcproj" TmcFilePath="TestPlcProject1\TestPlcProject1.tmc" ReloadTmc="true" AmsPort="851" FileArchiveSettings="#x000e" SymbolicMapping="true">
                				<Instance Id="#x08502000" TcSmClass="TComPlcObjDef" KeepUnrestoredLinks="2" TmcPath="TestPlcProject1\TestPlcProject1.tmc" TmcHash="{7B06BEE5-6CF9-9104-20B7-826712E47DEA}">
                					<Name>TestPlcProject1 Instance</Name>
                					<CLSID ClassFactory="TcPlc30">{08500001-0000-0000-F000-000000000064}</CLSID>
                					<Contexts>
                						<Context>
                							<Id>0</Id>
                							<Name>PlcTask</Name>
                							<ManualConfig>
                								<OTCID>#x02010030</OTCID>
                							</ManualConfig>
                							<Priority>20</Priority>
                							<CycleTime>10000000</CycleTime>
                						</Context>
                					</Contexts>
                					<TaskPouOids>
                						<TaskPouOid Prio="20" OTCID="#x08502001"/>
                					</TaskPouOids>
                				</Instance>
                			</Project>
                			<Project GUID="{A356EEB8-AAFC-4761-A0DD-0B8C83CA855D}" Name="TestPlcProject2" PrjFilePath="TestPlcProject2\TestPlcProject2.plcproj" TmcFilePath="TestPlcProject2\TestPlcProject2.tmc" ReloadTmc="true" AmsPort="852" FileArchiveSettings="#x000e" SymbolicMapping="true">
                				<Instance Id="#x08502040" TcSmClass="TComPlcObjDef" KeepUnrestoredLinks="2">
                					<Name>TestPlcProject2 Instance</Name>
                					<CLSID ClassFactory="TcPlc30">{08500001-0000-0000-F000-000000000064}</CLSID>
                					<Contexts>
                						<Context>
                							<Id>1</Id>
                							<Name>Default</Name>
                						</Context>
                					</Contexts>
                				</Instance>
                			</Project>
                		</Plc>
                	</Project>
                </TcSmProject>
                """,
                new Dictionary<string, string> { { "TcVersion", "3.1.4024.53" } },
                new List<string>() { "TestPlcProject1", "TestPlcProject2" }
            };

            yield return new object[]
            {
                """
                <?xml version="1.0"?>
                <TcSmProject xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:noNamespaceSchemaLocation="http://www.beckhoff.com/schemas/2012/07/TcSmProject" TcSmVersion="1.0" TcVersion="3.1.4024.53">
                	<Project ProjectGUID="{CD779436-E0BF-4C3D-9866-879794D3B6A4}" ShowHideConfigurations="#x3c7"/>
                </TcSmProject>
                """,
                new Dictionary<string, string> { { "TcVersion", "3.1.4024.53" } },
                new List<string>()
            };
        }
    }

    private readonly ITestOutputHelper _output = output;

    [Fact]
    public void PlcProjectBelongToSolution_WithRelatedFiles_ShouldBeTrue()
    {
        // Arrange
        using TestProject testProject = new();
        TestTwincatProject? testTcProject = testProject.TwincatProjects[0];
        TestPlcProject? testPlcProject = testTcProject.PlcProjects[0];

        // Act
        bool actual = TwincatUtils.PlcProjectBelongToSolution(
            testPlcProject.AbsolutePath,
            testProject.SolutionPath
        );

        // Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void PlcProjectBelongToSolution_WithUnrelatedFiles_ShouldBeFalse()
    {
        // Arrange
        using TestProject testProject1 = new();
        using TestProject testProject2 = new();
        TestTwincatProject? testTcProject = testProject1.TwincatProjects[0];
        TestPlcProject? testPlcProject = testTcProject.PlcProjects[0];

        // Act
        bool actual = TwincatUtils.PlcProjectBelongToSolution(
            testPlcProject.AbsolutePath,
            testProject2.SolutionPath
        );

        // Assert
        actual.Should().BeFalse();
    }

    [Theory]
    [InlineData("foo.tsproj", true)]
    [InlineData("foo.tspproj", true)]
    [InlineData(".", false)]
    [InlineData("foo.bar", false)]
    [InlineData("foo", false)]
    public void IsTwincatProjectFileExtension_ShouldWork(string filePath, bool expected)
    {
        bool actual = TwincatUtils.IsTwincatProjectFileExtension(filePath);

        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("foo.plcproj", true)]
    [InlineData("bar.plcproj", true)]
    [InlineData(".", false)]
    [InlineData("foo.bar", false)]
    [InlineData("foo", false)]
    public void IsPlcProjectFileExtension_ShouldWork(string filePath, bool expected)
    {
        bool actual = TwincatUtils.IsPlcProjectFileExtension(filePath);

        actual.Should().Be(expected);
    }

    [Theory]
    [MemberData(nameof(TestData.PlcProjectFiles), MemberType = typeof(TestData))]
    public void DeserializeXmlFileToProjectData_WithValidPlcProjectFile_ShouldSucceed(
        string xmlContent,
        Dictionary<string, string> expected
    )
    {
        // Arrange
        var projectFile = Path.GetTempFileName();
        File.WriteAllText(projectFile, xmlContent);

        // Act
        var plcProjectData = TwincatUtils.DeserializeXmlFileToProjectData<PlcProjectData>(
            projectFile
        );

        // Assert
        plcProjectData.Should().NotBeNull();
        foreach (var (property, value) in expected)
        {
            plcProjectData
                .GetType()
                .GetProperty(property)
                ?.GetValue(plcProjectData, null)
                .Should()
                .Be(value);
        }

        File.Delete(projectFile);
    }

    [Theory]
    [MemberData(nameof(TestData.TwincatProjectFiles), MemberType = typeof(TestData))]
    public async void DeserializeXmlFileToProjectDataAsync_WithValidTwincatProjectFile_ShouldSucceedAsync(
        string xmlContent,
        Dictionary<string, string> expectedProperties,
        List<string> plcProjectNames
    )
    {
        // Arrange
        var projectFile = Path.GetTempFileName();
        File.WriteAllText(projectFile, xmlContent);

        // Act
        TcSmProjectData tcProjectData =
            await TwincatUtils.DeserializeXmlFileToProjectDataAsync<TcSmProjectData>(projectFile);

        // Assert
        tcProjectData.Should().NotBeNull();
        foreach (var (property, value) in expectedProperties)
        {
            tcProjectData
                .GetType()
                .GetProperty(property)
                ?.GetValue(tcProjectData, null)
                .Should()
                .Be(value);
        }

        var actualPlcProjectNames =
            tcProjectData.Project.Plc?.Projects.Select(x => x.Name) ?? new List<string>();
        plcProjectNames.Should().BeEquivalentTo(actualPlcProjectNames);

        File.Delete(projectFile);
    }
}
