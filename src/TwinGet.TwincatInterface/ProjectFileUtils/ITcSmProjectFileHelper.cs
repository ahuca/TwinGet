// This file is licensed to you under MIT license.

namespace TwinGet.TwincatInterface.ProjectFileUtils;

public interface ITcSmProjectFileHelper : IProjectMetadata
{
    /// <summary>
    /// Get the parent solution.
    /// </summary>
    /// <param name="upwardDepth">The upward depth of directories to search, starting from the directory of <see cref="Path"/>.</param>
    /// <returns>The aboslute path of the parent solution if successful, otherwise <see cref="string.Empty"/>.</returns>
    public string GetParentSolutionFile(int upwardDepth);

    /// <summary>
    /// Get the parent solution.
    /// </summary>
    /// <param name="upwardDepth">The upward depth of directories to search, starting from the directory of <see cref="Path"/>.</param>
    /// <returns>The aboslute path of the parent solution if successful, otherwise <see cref="string.Empty"/>.</returns>
    public Task<string> GetParentSolutionFileAsync(int upwardDepth);
}
