// This file is licensed to you under MIT license.

namespace TwinGet.TwincatInterface.Utils;

internal static class DteExtensions
{
    /// <summary>
    /// Verify whether the current <see cref="EnvDTE.Project"/> is a TwinCAT project.
    /// </summary>
    /// <param name="project"></param>
    /// <returns>true if the <see cref="EnvDTE.Project"/> is a TwinCAT project, and false otherwise.</returns>
    public static bool IsTwincatProject(this EnvDTE.Project project)
    {
        bool isTwincatXaeProject = project.Kind == TwincatConstants.TwincatXaeProjectKind;
        bool isTwincatPlcProject = project.Kind == TwincatConstants.TwincatPlcProjectKind;
        return isTwincatXaeProject || isTwincatPlcProject;
    }

    /// <summary>
    /// Check if TwinCAT is integrated with the given DTE instance.
    /// </summary>
    /// <param name="dte"></param>
    /// <returns>True if TwinCAT is intergrated, false otherwise.</returns>
    internal static bool IsTwinCatIntegrated(this EnvDTE.DTE dte)
    {
        // DTE instance is a TcXaeShell, so no need for further check
        if (dte.Name == "TcXaeShell")
        {
            return true;
        }

        // DTE instance is a Visual Studio, hence we need to check that TwinCAT is integrated
        try
        {
            dte.GetObject("TcRemoteManager");
            return true;
        }
        catch { }

        return false;
    }
}
