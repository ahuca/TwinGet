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

            string actual = TwincatUtils.GetParentTwincatProject(testPlcProject.AbsolutePath);
            string expected = testTcProject.AbsolutePath;

            expected.Should().NotBeNullOrEmpty();
            expected.Should().Be(actual);
        }
    }
}
