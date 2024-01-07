// This file is licensed to you under MIT license.

namespace TwinGet.TwincatInterface.Dto;

public static class TcSmProjectDataExtensions
{
    public static bool HasProject(this TcSmProjectData tcSmProject) =>
        tcSmProject.Project.Plc is not null
        && tcSmProject.Project.Plc?.Projects is not null
        && tcSmProject.Project.Plc?.Projects.Count != 0;

    public static IEnumerable<ProjectElement> EnumerateProjects(this TcSmProjectData tcSmProject)
    {
        if (!tcSmProject.HasProject())
        {
            yield break;
        }

        if (tcSmProject.Project.Plc is not null)
        {
            foreach (var project in tcSmProject.Project.Plc.Projects)
            {
                yield return project;
            }
        }
    }
}
