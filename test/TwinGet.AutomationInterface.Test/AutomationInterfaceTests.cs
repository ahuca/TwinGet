namespace TwinGet.AutomationInterface.Test
{
    public class AutomationInterfaceTests : IDisposable
    {
        private bool disposedValue;

        [StaFact]
        public void ProgIdShouldBeValid()
        {
            var ai = new AutomationInterface();
            AutomationInterfaceConstants.SupportedProgIds.Should().Contain(ai.ProgId);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
                disposedValue = true;
            }
        }

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~AutomationInterfaceTests()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}