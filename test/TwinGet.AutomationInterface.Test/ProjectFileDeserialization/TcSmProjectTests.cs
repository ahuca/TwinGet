// This file is licensed to you under MIT license.

using System.Xml.Serialization;
using TwinGet.AutomationInterface.ProjectFileDeserialization;

namespace TwinGet.AutomationInterface.Test.ProjectFileDeserialization
{
    public class TcSmProjectTests
    {
        private class TestData
        {
            public static IEnumerable<object[]> ValidXmlContents()
            {
                yield return new object[] {
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
                    				<Instance Id="#x08502000" TcSmClass="TComPlcObjDef" KeepUnrestoredLinks="2" TmcPath="TestPlcProject\TestPlcProject.tmc" TmcHash="{7B06BEE5-6CF9-9104-20B7-826712E47DEA}">
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
                    "3.1.4024.53",
                    new List<string>(){ "TestPlcProject1", "TestPlcProject2" } };

                yield return new object[] {
                    """
                    <?xml version="1.0"?>
                    <TcSmProject xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:noNamespaceSchemaLocation="http://www.beckhoff.com/schemas/2012/07/TcSmProject" TcSmVersion="1.0" TcVersion="3.1.4024.53">
                    	<Project ProjectGUID="{CD779436-E0BF-4C3D-9866-879794D3B6A4}" ShowHideConfigurations="#x3c7"/>
                    </TcSmProject>
                    """,
                    "3.1.4024.53",
                    new List<string>() };
            }
        }

        [Theory]
        [MemberData(nameof(TestData.ValidXmlContents), MemberType = typeof(TestData))]
        public void Deserialize_FromValidXmlContent_ShouldSucceed(string xmlContent, string tcVersion, List<string> plcProjectNames)
        {
            XmlSerializer serializer = new(typeof(TcSmProjectData));

            using (StringReader reader = new(xmlContent))
            {
                TcSmProjectData? project = serializer.Deserialize(reader) as TcSmProjectData;

                project.Should().NotBeNull();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                project.TcSmVersion.Should().NotBeNullOrEmpty();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                project.TcVersion.Should().Be(tcVersion);
                if (plcProjectNames.Count > 0)
                {
                    project.Project.Plc.Projects.Count.Should().Be(plcProjectNames.Count);
                    project.Project.Plc.Projects.Select(x => x.Name).Should().BeEquivalentTo(plcProjectNames);
                }
                else
                {
                    project.Project.Plc.Should().BeNull();
                }

            }
        }
    }
}
