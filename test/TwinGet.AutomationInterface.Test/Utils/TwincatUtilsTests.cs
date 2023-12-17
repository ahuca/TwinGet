// This file is licensed to you under MIT license.

using TwinGet.AutomationInterface.Test.TestUtils;
using TwinGet.AutomationInterface.Utils;

namespace TwinGet.AutomationInterface.Test.Utils
{
    public class TwincatUtilsTests
    {
        [Fact]
        public void GetParentTwincatProject_ShouldSucceed()
        {
            // Arrange
            using TestProject testProject = new TestProject();
            TestTwincatProject? testTcProject = null;
            TestPlcProject? testPlcProject = null;

            // Find a test TwinCAT project that has at least one PLC project.
            foreach (TestTwincatProject tcProj in testProject.TwincatProjects)
            {
                if (tcProj.PlcProjects.Count > 0)
                {
                    testTcProject = tcProj;
                    testPlcProject = tcProj.PlcProjects.First();
                }
            }

            testTcProject.Should().NotBeNull();
            string expected = testTcProject.AbsolutePath;

            // Act
            string actual = TwincatUtils.GetParentTwincatProjectFile(testPlcProject.AbsolutePath);

            // Assert
            expected.Should().NotBeNullOrEmpty();
            expected.Should().Be(actual);
        }

        [Fact]
        public void GetParentSolutionFile_ShouldSucceed()
        {
            // Arrange
            using TestProject testProject = new();
            TestTwincatProject? testTcProject = testProject.TwincatProjects[0];

            string expected = testProject.SolutionPath;

            // Act
            string actual = TwincatUtils.GetParentSolutionFile(testTcProject.AbsolutePath);

            // Assert
            expected.Should().Be(actual);
        }
    }
}
