// This file is licensed to you under MIT license.

namespace TwinGet.AutomationInterface.Test
{
    public class AutomationInterfaceTests : IDisposable
    {
        private bool _disposedValue;
        private readonly string _testDirectory;
        private readonly string _testTwincatSolution;

        private static string ProvisionTestDirectory()
        {
            string testDirectory = Path.Join(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(testDirectory);

            return testDirectory;
        }

        private void TearDownTestDirectory()
        {
            if (Directory.Exists(_testDirectory)) { Directory.Delete(_testDirectory, true); }
        }

        private static void CopyTestTwincatProject(string destination)
        {
            TwinGet.Utils.IO.Directory.CopyDirectory(
                TestTwincatSolutionConstants.s_testTwincatProject,
                destination).Wait();
        }

        public AutomationInterfaceTests()
        {
            _testDirectory = ProvisionTestDirectory();

            CopyTestTwincatProject(_testDirectory);

            _testTwincatSolution = Directory.GetFiles(_testDirectory, "*.sln", SearchOption.AllDirectories).First();
        }

        [Fact]
        public void PathExists_ShouldSupportRelativePath()
        {
            Path.Exists(@"TestTwincatProject\TestTwincatProject.sln").Should().BeTrue();
        }

        [StaFact]
        public void ProgId_ShouldNotBeNullOrEmpty()
        {
            var sut = new AutomationInterface();

            sut.ProgId.Should().NotBeNullOrEmpty();
        }

        [StaFact]
        public void ProgId_ShouldBeValid()
        {
            var sut = new AutomationInterface();

            AutomationInterfaceConstants.SupportedProgIds.Should().Contain(sut.ProgId);
        }


        [StaFact]
        public void LoadSolution_WithValidPath_ShouldLoadSuccessfully()
        {
            var sut = new AutomationInterface();
            sut.LoadSolution(_testTwincatSolution);

            sut.LoadedSolutionFile.Should().Be(_testTwincatSolution);
            sut.IsSolutionOpen.Should().BeTrue();
        }

        [StaFact]
        public void LoadSolution_WithInvalidPath_ShouldThrow()
        {
            var sut = new AutomationInterface();

            string invalidSolution = $"{Guid.NewGuid()}.sln";
            Action loadSolution = () => sut.LoadSolution(invalidSolution);

            loadSolution.Should().Throw<FileNotFoundException>();
            sut.IsSolutionOpen.Should().BeFalse();
        }

        [StaFact]
        public void LoadSolution_WithEmptyPath_ShouldThrow()
        {
            var sut = new AutomationInterface();

            Action loadSolution = () => sut.LoadSolution(string.Empty);

            loadSolution.Should().Throw<ArgumentException>();
            sut.IsSolutionOpen.Should().BeFalse();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    ;
                }

                TearDownTestDirectory();
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
