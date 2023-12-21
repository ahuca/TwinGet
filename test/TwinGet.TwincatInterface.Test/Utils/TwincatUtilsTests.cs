// This file is licensed to you under MIT license.

using TwinGet.TwincatInterface.Utils;

namespace TwinGet.TwincatInterface.Test.Utils
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

        [Fact]
        public void PlcProjectBelongToSolution_ShouldWork()
        {
            // Arrange
            using TestProject testProject = new();
            TestTwincatProject? testTcProject = testProject.TwincatProjects[0];
            TestPlcProject? testPlcProject = testTcProject.PlcProjects[0];

            // Act
            bool actual = TwincatUtils.PlcProjectBelongToSolution(testPlcProject.AbsolutePath, testProject.SolutionPath);

            // Assert
            actual.Should().BeTrue();
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
    }
}
