// This file is licensed to you under MIT license.

namespace TwinGet.TwincatInterface.Dto;

public static class TcSmProjectDataExtensions
{
    public static bool HasProject(this TcSmProjectData tcScProject) =>
        tcScProject.Project.Plc is not null
        && tcScProject.Project.Plc?.Projects is not null
        && tcScProject.Project.Plc?.Projects.Count != 0;
}
