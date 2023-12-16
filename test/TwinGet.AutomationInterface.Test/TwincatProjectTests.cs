// This file is licensed to you under MIT license.

using TwinGet.AutomationInterface.Exceptions;
using TwinGet.AutomationInterface.Test.TestUtils;
using TwinGet.AutomationInterface.Utils;
using static TwinGet.AutomationInterface.TwincatConstants;

namespace TwinGet.AutomationInterface.Test
{
    public class TwincatProjectTests : IDisposable
    {
        private bool _disposedValue;

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
            using TestProject testProject = new();
            using TwincatDteProvider dteProvider = new(this);
            dteProvider.Dte.Solution.Open(testProject.SolutionPath);
            EnvDTE.Project? project = GetFirstTwincatProject(dteProvider.Dte.Solution.Projects);

            // "Assert" no exception
#pragma warning disable CS8604 // Possible null reference argument.
            TwincatProject? tcProject = new(project);
#pragma warning restore CS8604 // Possible null reference argument.
        }

        [StaFact]
        public void Construct_WithNonTwincatProject_ShouldThrow()
        {
            // Arrange
            using TestProject testProject = new();
            using TwincatDteProvider dteProvider = new(this);
            dteProvider.Dte.Solution.Open(testProject.SolutionPath);
            EnvDTE.Project? project = GetFirstNonTwincatProject(dteProvider.Dte.Solution.Projects);

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
            using TestProject testProject = new();
            using TwincatDteProvider dteProvider = new(this);
            dteProvider.Dte.Solution.Open(testProject.SolutionPath);

            // Act, construct TwincatProjects
            List<TwincatProject> twincatProjects = [];
            for (int i = ProjectItemStartingIndex; i <= dteProvider.Dte.Solution.Projects.Count; i++)
            {
                EnvDTE.Project currentProject = dteProvider.Dte.Solution.Projects.Item(i);
                if (currentProject.IsTwincatProject())
                    twincatProjects.Add(new TwincatProject(currentProject));
            }

            // Assert
            foreach (TwincatProject twincatProject in twincatProjects)
            {
                TestTwincatProject? testTwincatProject = testProject.TwincatProjects.Where(x => x.Name == twincatProject.Name).FirstOrDefault();

                IEnumerable<string>? expectedPlcProjectNames = testTwincatProject?.PlcProjects.Select(x => x.Name);
                IEnumerable<string> actualPlcProjectNames = twincatProject.PlcProjects.Select(x => x.Name);

                expectedPlcProjectNames.Should().BeEquivalentTo(actualPlcProjectNames);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing) { }

                GC.Collect();
                GC.WaitForPendingFinalizers();
                _disposedValue = true;
            }
        }

        ~TwincatProjectTests()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
