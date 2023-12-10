// This file is licensed to you under MIT license.

using TwinGet.AutomationInterface.Exceptions;
using TwinGet.AutomationInterface.Utils;

namespace TwinGet.AutomationInterface.Test
{
    public class TwincatProjectTests
    {
        private static EnvDTE.Project? GetFirstTwincatProject(EnvDTE.Projects projects)
        {
            for (int i = 1; i <= projects.Count; i++)
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
            for (int i = 1; i <= projects.Count; i++)
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
    }
}
