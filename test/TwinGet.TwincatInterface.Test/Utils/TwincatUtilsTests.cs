// This file is licensed to you under MIT license.

using System.Collections.Frozen;
using TwinGet.TwincatInterface.Dto;
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

        [Fact]
        public void DeserializeXmlFileToProjectData_WithValidTwincatProjectFile_ShouldSucceed()
        {
            using TestProject testProject = new();

            TcSmProjectData plcProjectData = TwincatUtils.DeserializeXmlFileToProjectData<TcSmProjectData>(testProject.TwincatProjects[0].AbsolutePath);

            plcProjectData.Should().NotBeNull();
        }

        [Fact]
        public async void DeserializeXmlFileToProjectDataAsync_WithValidPlcProjectFile_ShouldSucceed()
        {
            using TestProject testProject = new();
            var plcProjects = testProject.GetPlcProjects();

            PlcProjectData plcProjectData = await TwincatUtils.DeserializeXmlFileToProjectDataAsync<PlcProjectData>(plcProjects.First().AbsolutePath);

            plcProjectData.Should().NotBeNull();
        }
    }
}
