// This file is licensed to you under MIT license.

namespace TwinGet.AutomationInterface.Test
{
    public class AutomationInterfaceTests : IDisposable
    {
        private bool _disposedValue;

        public AutomationInterfaceTests()
        {

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
            sut.LoadSolution(TestTwincatSolutionConstants.s_testTwincatSolution);

            sut.LoadedSolutionFile.Should().Be(TestTwincatSolutionConstants.s_testTwincatSolution);
            sut.IsSolutionOpen.Should().BeTrue();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    ;
                }

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
