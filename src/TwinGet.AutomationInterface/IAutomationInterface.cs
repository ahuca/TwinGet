// This file is licensed to you under MIT license.

namespace TwinGet.AutomationInterface
{
    public interface IAutomationInterface
    {
        public string ProgId { get; }
        public bool IsSolutionOpen { get; }
        public string LoadedSolutionFile { get; }
        public IReadOnlyList<ITwincatProject> TwincatProjects { get; }

        public void LoadSolution(string filePath);

    }
}
