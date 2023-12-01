using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using TwinGet.AutomationInterface.ComMessageFilter;
using TwinGet.AutomationInterface.Exceptions;
using TwinGet.AutomationInterface.Utils;

namespace TwinGet.AutomationInterface
{
    [SupportedOSPlatform("windows")]
    public class AutomationInterface : IDisposable
    {
        public string ProgId { get; private set; }
        private EnvDTE80.DTE2? _dte;
        private bool disposedValue;

        public AutomationInterface()
        {
            ProgId = String.Empty; // To avoid CS8618
            MessageFilter.Register();
            TryInitializeDte();
            if (_dte is null) { throw new CouldNotCreateTwinCatDte("Is TwinCAT installed in this system?"); }
            Console.CancelKeyPress += new ConsoleCancelEventHandler(DisposeOnCancelEvent);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    ;
                }

                CleanUp();
                disposedValue = true;
            }
        }

        ~AutomationInterface()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected void DisposeOnCancelEvent(object sender, ConsoleCancelEventArgs args)
        {
            Dispose();
        }

        private void CleanUp()
        {
            ProgId = String.Empty;

            if (_dte is not null)
            {
                _dte.Quit();
                Marshal.ReleaseComObject(_dte);
                MessageFilter.Revoke();
            }
        }

        /// <summary>
        /// Try to create a Visual Studio (or TwinCAT XAE) DTE instance.
        /// </summary>
        /// <returns>true if successful, otherwise false</returns>
        private bool TryInitializeDte()
        {
            foreach (var p in AutomationInterfaceConstants.SupportedProgIds)
            {
                Type? t = Type.GetTypeFromProgID(p);

                if (t is null) { continue; }

                EnvDTE80.DTE2? dte;
                try
                {
                    dte = (EnvDTE80.DTE2?)Activator.CreateInstance(t);
                }
                catch { continue; }

                if (dte is null)
                {
                    continue;
                }

                if (dte.IsTwinCatIntegrated())
                {
                    ProgId = p;
                    _dte = dte;
                    return true;
                }
            }

            ProgId = String.Empty;
            _dte = null;
            return false;
        }

    }
}
