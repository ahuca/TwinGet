// This file is licensed to you under MIT license.

namespace TwinGet.Utils.Test.IOTests;

public class DirectoryTests
{
    private enum PathType
    {
        Directory = 0,
        File = 1
    }

    private static readonly Dictionary<string, PathType> s_testDirContent =
        new()
        {
            { @"folder1", PathType.Directory },
            { @"folder1/file1.txt", PathType.File },
            { @"folder2", PathType.Directory },
            { @"folder2/file1.txt", PathType.File },
            { @"folder2/folder1", PathType.Directory },
            { @"folder2/folder1/folder1", PathType.Directory },
            { @"folder2/folder1/folder1/file1.txt", PathType.File },
            { @"folder2/folder1/folder1/file2.txt", PathType.File },
            { @"file1.txt", PathType.File },
            { @"file2.txt", PathType.File },
        };

    /// <summary>
    /// Initialize a test directory and return the root path.
    /// </summary>
    /// <returns>The root path of the test directory.</returns>
    private static string ProvisionTestDirectory(Dictionary<string, PathType> directoryStructure)
    {
        string testDirectory = Path.Join(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(testDirectory);

        static void createItem(PathType type, string path)
        {
            switch (type)
            {
                case PathType.Directory:
                    Directory.CreateDirectory(path);
                    break;
                case PathType.File:
                    File.Create(path)?.Close();
                    break;
            }
        }

        foreach (KeyValuePair<string, PathType> content in directoryStructure)
        {
            string fullPath = Path.Join(testDirectory, content.Key);
            createItem(content.Value, fullPath);
        }

        return testDirectory;
    }

    [Fact]
    public async void CopyDirectory_Recursively()
    {
        // Arrange
        string sourceDirectory = ProvisionTestDirectory(s_testDirContent);
        string destinationDirectory = Path.Join(Path.GetTempPath(), Guid.NewGuid().ToString());

        // Act
        await IO.Directory.CopyDirectory(sourceDirectory, destinationDirectory);

        // Assert
        DirectoryComparer.Equals(destinationDirectory, sourceDirectory).Should().BeTrue();

        // Cleanup
        Directory.Delete(destinationDirectory, true);
        Directory.Delete(sourceDirectory, true);
    }
}
