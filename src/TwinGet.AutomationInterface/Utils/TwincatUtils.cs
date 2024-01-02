// This file is licensed to you under MIT license.

using TCatSysManagerLib;
using TwinGet.AutomationInterface.Exceptions;
using ITcSmTreeItemAlias = TCatSysManagerLib.ITcSmTreeItem9;
using ITcSysManagerAlias = TCatSysManagerLib.ITcSysManager15;

namespace TwinGet.AutomationInterface.Utils
{
    internal static class TwincatUtils
    {
        public static ITcPlcIECProject3? LookUpPlcProject(this ITcSysManagerAlias systemManager, string name)
        {
            ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));

            var treeItem = (ITcSmTreeItemAlias)systemManager.LookupTreeItem($"TIPC^{name}^{name} Project");
            var plcProject = (ITcPlcIECProject3)treeItem ?? throw new CouldNotLookUpPlcProject($"Could not look up any PLC project with the provided name", name);

            return plcProject;
        }
    }
}
