// This file is licensed to you under MIT license.

namespace TwinGet.AutomationInterface
{
    public interface ITwincatProject : EnvDTE.Project
    {
        public IReadOnlyList<IPlcProject> PlcProjects { get; }
    }
}
