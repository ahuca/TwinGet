// This file is licensed to you under MIT license.

namespace TwinGet.AutomationInterface.Test
{
    public class AutomationInterfaceTests : IDisposable
    {
        private bool _disposedValue;

        [StaFact]
        public void ProgId_ShouldNotBeNullOrEmpty()
        {
            var ai = new AutomationInterface();
            ai.ProgId.Should().NotBeNullOrEmpty();
        }

        [StaFact]
        public void ProgId_ShouldBeValid()
        {
            var ai = new AutomationInterface();
            AutomationInterfaceConstants.SupportedProgIds.Should().Contain(ai.ProgId);
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
