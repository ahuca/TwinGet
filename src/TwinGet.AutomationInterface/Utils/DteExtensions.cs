using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TwinGet.AutomationInterface.Utils
{
    internal static class DteExtensions
    {
        internal static bool IsTwinCatIntegrated(this EnvDTE.DTE dte)
        {
            try
            {
                dte.GetObject("TcRemoteManager");
                return true;
            }
            catch { }

            return false;
        }
    }
}
