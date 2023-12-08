// This file is licensed to you under MIT license.

using TwinGet.AutomationInterface.Exceptions;

namespace TwinGet.AutomationInterface.Test
{
    public class TwincatProjectTests
    {
        private const string _nonTwincatProjectNamePrefix = "TestNonTwincatProject";
        private const string _twincatProjectNamePrefix = "TestTwincatProject";

        private EnvDTE.Project? GetFirstTwincatProject(EnvDTE.Projects projects)
        {
            for (int i = 1; i <= projects.Count; i++)
            {
                EnvDTE.Project tmp = projects.Item(i);

                if (tmp.Name.Contains(_twincatProjectNamePrefix))
                {
                    return tmp;
                }
            }

            return null;
        }

        private EnvDTE.Project? GetFirstNonTwincatProject(EnvDTE.Projects projects)
        {
            for (int i = 1; i <= projects.Count; i++)
            {
                EnvDTE.Project tmp = projects.Item(i);

                if (tmp.Name.Contains(_nonTwincatProjectNamePrefix))
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
            EnvDTE.Project? project = GetFirstTwincatProject(dte.Solution.Projects);

            // "Assert" no exception
            TwincatProject? tcProject = new(project);
        }

        [StaFact]
        public void Construct_WithNonTwincatProject_ShouldThrow()
        {
            // Arrange
            EnvDTE80.DTE2? dte = null;
            using TestProject testProject = new();
            using AutomationInterface ai = new(ref dte);
            ai.LoadSolution(testProject.SolutionPath);
            EnvDTE.Project? project = GetFirstNonTwincatProject(dte.Solution.Projects);

            // Act
            Action constructTwincatProject = () => { TwincatProject? tcProject = new(project); };

            // Assert
            constructTwincatProject.Should().Throw<NotATwincatProject>();
        }
    }
}
