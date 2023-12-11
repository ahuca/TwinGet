// This file is licensed to you under MIT license.

using TwinGet.AutomationInterface.Test.TestUtils;

namespace TwinGet.AutomationInterface.Test
{
    public class AutomationInterfaceTests : IDisposable
    {
        private bool _disposedValue;
        [Fact]
        public void PathExists_ShouldSupportRelativePath()
        {
            Path.Exists(@"TestTwincatProject\TestTwincatProject.sln").Should().BeTrue();
        }

        [StaFact]
        public void ProgId_ShouldNotBeNullOrEmpty()
        {
            using AutomationInterface sut = new();

            sut.ProgId.Should().NotBeNullOrEmpty();
        }

        [StaFact]
        public void ProgId_ShouldBeValid()
        {
            using AutomationInterface sut = new();

            AutomationInterfaceConstants.SupportedProgIds.Should().Contain(sut.ProgId);
        }


        [StaFact]
        public void LoadSolution_WithValidPath_ShouldLoadSuccessfully()
        {
            using AutomationInterface sut = new();
            using TestProject project = new();

            sut.LoadSolution(project.SolutionPath);

            sut.LoadedSolutionFile.Should().Be(project.SolutionPath);
            sut.IsSolutionOpen.Should().BeTrue();
        }

        [StaFact]
        public void LoadSolution_WithInvalidPath_ShouldThrow()
        {
            using AutomationInterface sut = new();

            string invalidSolution = $"{Guid.NewGuid()}.sln";
            Action loadSolution = () => sut.LoadSolution(invalidSolution);

            loadSolution.Should().Throw<FileNotFoundException>();
            sut.IsSolutionOpen.Should().BeFalse();
        }

        [StaFact]
        public void LoadSolution_WithEmptyPath_ShouldThrow()
        {
            using AutomationInterface sut = new();

            Action loadSolution = () => sut.LoadSolution(string.Empty);

            loadSolution.Should().Throw<ArgumentException>();
            sut.IsSolutionOpen.Should().BeFalse();
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

        ~AutomationInterfaceTests()
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
