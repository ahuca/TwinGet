// This file is licensed to you under MIT license.

namespace TwinGet.TwincatInterface
{
    public interface ITwincatProject : EnvDTE.Project
    {
        /// <summary>
        /// A <see cref="IReadOnlyList{IPlcProject}"/>.
        /// </summary>
        public IReadOnlyList<IPlcProject> PlcProjects { get; }
    }
}
