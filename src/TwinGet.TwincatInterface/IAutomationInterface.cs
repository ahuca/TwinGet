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

        /// <summary>
        /// Save a PLC project into library to a specified directory.
        /// </summary>
        /// <param name="plcProjectPath">The path to the PLC project file.</param>
        /// <param name="solutionPath">The path to the solution containing the <paramref name="plcProjectPath"/>.</param>
        /// <param name="outputDirectory">The output directory for the library file.</param>
        /// <returns>The <see cref="IPlcProject"/> that was saved successfully, otherwise null.</returns>
        public IPlcProject? SavePlcProject(string plcProjectPath, string outputDirectory, string solutionPath = "");

        /// <summary>
        /// Get all the <see cref="IPlcProject"/> that belongs to the current loaded solution.
        /// </summary>
        /// <returns>All the <see cref="IPlcProject"/> if any, otherwise an empty <see cref="System.Collections.IEnumerable"/></returns>
        public IEnumerable<IPlcProject> GetPlcProjects();
    }
}
