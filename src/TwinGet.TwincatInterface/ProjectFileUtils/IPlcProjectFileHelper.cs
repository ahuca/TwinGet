// This file is licensed to you under MIT license.

namespace TwinGet.TwincatInterface.ProjectFileUtils;

public interface IPlcProjectFileHelper : IProjectMetadata
{
    /// <summary>
    /// Get the parent TwinCAT project.
    /// </summary>
    /// <param name="upwardDepth">The upward depth of directories to search, starting from the directory of <see cref="Path"/>.</param>
    /// <returns>The aboslute path of the parent TwinCAT project if successful, otherwise <see cref="string.Empty"/>.</returns>
    public string GetParentTwincatFile(int upwardDepth = 5);

    /// <summary>
    /// Get the parent TwinCAT project.
    /// </summary>
    /// <param name="upwardDepth">The upward depth of directories to search, starting from the directory of <see cref="Path"/>.</param>
    /// <returns>The aboslute path of the parent TwinCAT project if successful, otherwise <see cref="string.Empty"/>.</returns>
    public Task<string> GetParentTwincatFileAsync(int upwardDepth = 5);

    /// <summary>
    /// Get the parent solution file.
    /// </summary>
    /// <param name="upwardDepth">The upward depth of directories to search.</param>
    /// <returns>The asbolute path of the parent solution if successful, otherwise <see cref="string.Empty"/>.</returns>
    public string GetParentSolutionFile(int upwardDepth = 5);

    /// <summary>
    /// Get the parent solution file.
    /// </summary>
    /// <param name="upwardDepth">The upward depth of directories to search.</param>
    /// <returns>The asbolute path of the parent solution if successful, otherwise <see cref="string.Empty"/>.</returns>
    public Task<string> GetParentSolutionFileAsync(int upwardDepth = 5);
}
