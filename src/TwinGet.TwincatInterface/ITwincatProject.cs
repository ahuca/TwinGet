// This file is licensed to you under MIT license.

namespace TwinGet.TwincatInterface
{
    public interface ITwincatProject : EnvDTE.Project
    {
        /// <summary>
        /// The absolute path to the project file (.tsproj or .tspproj).
        /// </summary>
        public string AbsolutePath { get; }

        /// <summary>
        /// A <see cref="IReadOnlyList{IPlcProject}"/>.
        /// </summary>
        public IReadOnlyList<IPlcProject> PlcProjects { get; }
    }
}
