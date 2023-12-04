// This file is licensed to you under MIT license.

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
        public string ProgId { get => _progId; }
        private string _progId;
        private readonly EnvDTE80.DTE2? _dte;
        private bool _disposedValue;

        public AutomationInterface()
        {
            MessageFilter.Register();
            _dte = TryInitializeDte(out _progId);
            if (_dte is null) { throw new CouldNotCreateTwinCatDte("Is TwinCAT installed in this system?"); }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    ;
                }

                CleanUp();
                _disposedValue = true;
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

        private void CleanUp()
        {
            _progId = string.Empty;

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
        /// <param name="progId">The ProgId of the created instance if successful, empty string if not.</param>
        /// <returns>The created DTE instance if successful, null if not.</returns>
        private static EnvDTE80.DTE2? TryInitializeDte(out string progId)
        {
            foreach (string p in AutomationInterfaceConstants.SupportedProgIds)
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
                    progId = p;
                    return dte;
                }
            }

            progId = string.Empty;
            return null;
        }

    }
}
