// This file is licensed to you under MIT license.

namespace TwinGet.AutomationInterface.Test.TestUtils
{
    public class TestTwincatProjectTests
    {
        [Fact]
        public void TestTwincatProject_ShouldBeValid()
        {
            using TestProject testProject = new();

            testProject.RootPath.Should().NotBeNullOrEmpty();
            testProject.SolutionPath.Should().NotBeNullOrEmpty();
            testProject.SolutionFile.Should().NotBeNull();
            testProject.TwincatProjects.Should().NotBeEmpty();

            foreach (TestTwincatProject tcProject in testProject.TwincatProjects)
            {
                tcProject.Should().NotBeNull();
                tcProject.Name.Should().NotBeNullOrEmpty();
                tcProject.AbsolutePath.Should().NotBeNullOrEmpty();

                foreach (TestPlcProject plcProject in tcProject.PlcProjects)
                {
                    plcProject.Should().NotBeNull();
                    plcProject.Name.Should().NotBeNullOrEmpty();
                    plcProject.AbsolutePath.Should().NotBeNullOrEmpty();
                }
            }
        }
    }
}
