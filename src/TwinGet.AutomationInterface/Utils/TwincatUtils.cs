// This file is licensed to you under MIT license.

namespace TwinGet.AutomationInterface.Utils
{
    internal static class TwincatUtils
    {
        public static bool IsTwincatProject(EnvDTE.Project project)
        {
            bool isTwincatXaeProject = project.Kind == AutomationInterfaceConstants.TwincatXaeProjectKind;
            bool isTwincatPlcProject = project.Kind == AutomationInterfaceConstants.TwincatPlcProjectKind;
            return isTwincatXaeProject || isTwincatPlcProject;
        }
    }
}
