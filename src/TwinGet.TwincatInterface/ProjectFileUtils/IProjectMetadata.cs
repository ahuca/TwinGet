// This file is licensed to you under MIT license.

namespace TwinGet.TwincatInterface.ProjectFileUtils;

public interface IProjectMetadata
{
    /// <summary>
    /// The absolute path to the project file.
    /// </summary>
    public string Path { get; }

    /// <summary>
    /// The direcotry of this project file.
    /// </summary>
    public string Directory { get; }

    /// <summary>
    /// Get the GUID of this project. See <see cref="Path"/>.
    /// </summary>
    /// <returns>The GUID found.</returns>
    public string Guid();

    /// <summary>
    /// Get the GUID of this project. See <see cref="Path"/>.
    /// </summary>
    /// <returns>The GUID found.</returns>
    public Task<string> GuidAsync();
}
