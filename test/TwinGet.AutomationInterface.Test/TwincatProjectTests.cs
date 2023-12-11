// This file is licensed to you under MIT license.

using TwinGet.AutomationInterface.Exceptions;
using TwinGet.AutomationInterface.Test.TestUtils;
using TwinGet.AutomationInterface.Utils;
using static TwinGet.AutomationInterface.AutomationInterfaceConstants;

namespace TwinGet.AutomationInterface.Test
{
    public class TwincatProjectTests
    {
        private static EnvDTE.Project? GetFirstTwincatProject(EnvDTE.Projects projects)
        {
            for (int i = ProjectItemStartingIndex; i <= projects.Count; i++)
            {
                EnvDTE.Project tmp = projects.Item(i);

                if (tmp.IsTwincatProject())
                {
                    return tmp;
                }
            }

            return null;
        }

        private static EnvDTE.Project? GetFirstNonTwincatProject(EnvDTE.Projects projects)
        {
            for (int i = ProjectItemStartingIndex; i <= projects.Count; i++)
            {
                EnvDTE.Project tmp = projects.Item(i);

                if (!tmp.IsTwincatProject())
                {
                    return tmp;
                }
            }

            return null;
        }

        [StaFact]
        public void Construct_WithTwincatProject_ShouldSucceed()
        {
            // Arrange
            EnvDTE80.DTE2? dte = null;
            using TestProject testProject = new();
            using AutomationInterface ai = new(ref dte);
            ai.LoadSolution(testProject.SolutionPath);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            EnvDTE.Project? project = GetFirstTwincatProject(dte.Solution.Projects);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            // "Assert" no exception
#pragma warning disable CS8604 // Possible null reference argument.
            TwincatProject? tcProject = new(project);
#pragma warning restore CS8604 // Possible null reference argument.
        }

        [StaFact]
        public void Construct_WithNonTwincatProject_ShouldThrow()
        {
            // Arrange
            EnvDTE80.DTE2? dte = null;
            using TestProject testProject = new();
            using AutomationInterface ai = new(ref dte);
            ai.LoadSolution(testProject.SolutionPath);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            EnvDTE.Project? project = GetFirstNonTwincatProject(dte.Solution.Projects);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            // Act
#pragma warning disable CS8604 // Possible null reference argument.
            Action constructTwincatProject = () => { TwincatProject? tcProject = new(project); };
#pragma warning restore CS8604 // Possible null reference argument.

            // Assert
            constructTwincatProject.Should().Throw<NotATwincatProject>();
        }

        [StaFact]
        public void PlcProjects_ShouldBeExpected()
        {
            // Arrange
            EnvDTE80.DTE2? dte = null;
            using TestProject testProject = new();
            using AutomationInterface ai = new(ref dte);
            ai.LoadSolution(testProject.SolutionPath);

            // Act, construct TwincatProjects
            List<TwincatProject> twincatProjects = [];
            for (int i = ProjectItemStartingIndex; i <= dte.Solution.Projects.Count; i++)
            {
                EnvDTE.Project currentProject = dte.Solution.Projects.Item(i);
                if (currentProject.IsTwincatProject())
                    twincatProjects.Add(new TwincatProject(currentProject));
            }

            // Assert
            foreach (TwincatProject twincatProject in twincatProjects)
            {
                TestTwincatProject? testTwincatProject = testProject.TwincatProjects.Where(x => x.Name == twincatProject.Name).FirstOrDefault();

                IEnumerable<string> expectedPlcProjectNames = testTwincatProject.PlcProjects.Select(x => x.Name);
                IEnumerable<string> actualPlcProjectNames = twincatProject.PlcProjects.Select(x => x.Name);

                expectedPlcProjectNames.Should().BeEquivalentTo(actualPlcProjectNames);
            }
        }
    }
}
