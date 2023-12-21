// This file is licensed to you under MIT license.

namespace TwinGet.TwincatInterface
{
    public interface IAutomationInterface
    {
        /// <summary>
        /// Program ID of the DTE instance
        /// </summary>
        public string ProgId { get; }

        /// <summary>
        /// Wheter a solution is open.
        /// </summary>
        public bool IsSolutionOpen { get; }

        /// <summary>
        /// The absolute path of the loaded solution.
        /// </summary>
        public string LoadedSolutionFile { get; }

        public IReadOnlyList<ITwincatProject> TwincatProjects { get; }

        /// <summary>
        /// Load a solution.
        /// </summary>
        /// <param name="filePath">The path to the solution to load.</param>
        public void LoadSolution(string filePath);

    }
}
