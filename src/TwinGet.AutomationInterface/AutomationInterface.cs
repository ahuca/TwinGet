using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using TwinGet.AutomationInterface.ComMessageFilter;
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
            MessageFilter.Revoke();
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private void CleanUp()
        {
            ProgId = String.Empty;

            if (_dte is not null)
            {
                _dte.Quit();
                Marshal.ReleaseComObject(_dte);
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
