using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwinGet.AutomationInterface
{
    public static class AutomationInterfaceConstants
    {
        public static readonly IReadOnlyList<string> SupportedProgIds = new List<string> {
            "TcXaeShell.DTE.15.0",
            "VisualStudio.DTE.12.0",
            "VisualStudio.DTE.14.0",
            "VisualStudio.DTE.15.0"
        };

    }
}
