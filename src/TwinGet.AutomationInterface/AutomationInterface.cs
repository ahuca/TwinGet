using EnvDTE;
using TwinGet.AutomationInterface.Utils;

namespace TwinGet.AutomationInterface
{
    public class AutomationInterface : IDisposable
    {
        public string ProgId { get; private set; }
        private EnvDTE80.DTE2 _dte;

        public AutomationInterface()
        {
            TryInitializeDte();
        }

        ~AutomationInterface()
        {
            Console.WriteLine("Cleaning AI up");
            if (_dte is not null)
            {
                _dte.Quit();
            }
        }

        private void TryInitializeDte()
        {
            foreach (var p in AutomationInterfaceConstants.SupportedProgIds)
            {
                Type t = Type.GetTypeFromProgID(p);

                EnvDTE80.DTE2 dte;
                try
                {
                    dte = (EnvDTE80.DTE2)Activator.CreateInstance(t);
                }
                catch { continue; }

                if (dte.IsTwinCatIntegrated())
                {
                    ProgId = p;
                    _dte = dte;
                    return;
                }
            }

            ProgId = String.Empty;
            _dte = null;
        }

        public void Dispose()
        {
            if (_dte is not null)
            {
                _dte.Quit();
            }
        }
    }
}
