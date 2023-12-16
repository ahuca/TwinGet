// This file is licensed to you under MIT license.

namespace TwinGet.Utils.IO
{
    public static class Directory
    {
        /// <summary>
        /// Method for copying a directory content.<see href="https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories"/>
        /// </summary>
        /// <param name="sourceDir">Source directory.</param>
        /// <param name="destinationDir">Destination directory.</param>
        /// <param name="share">A <see cref="FileShare"/>> value specifying the type of access other threads have to the file.</param>
        /// <param name="recursive">If true, copy the contents of all subdirectories. If false, copy only the contents of the current directory</param>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public static async Task CopyDirectory(string sourceDir, string destinationDir, FileShare share = FileShare.None, bool recursive = true)
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            // Create the destination directory
            System.IO.Directory.CreateDirectory(destinationDir);


            // Get the files in the source directory and copy to the destination directory
            foreach (string filename in System.IO.Directory.EnumerateFiles(sourceDir))
            {
                using (FileStream sourceStream = File.Open(filename, FileMode.Open, FileAccess.Read, share))
                {
                    using (FileStream destinationStream = File.Create(Path.Combine(destinationDir, Path.GetFileName(filename))))
                    {
                        await sourceStream.CopyToAsync(destinationStream);
                    }
                }
            }

            // Cache directories before we start copying
            DirectoryInfo[] dirs = dir.GetDirectories();

            // If recursive and copying subdirectories, recursively call this method
            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    await CopyDirectory(subDir.FullName, newDestinationDir, FileShare.Read, true);
                }
            }

            return;
        }
    }
}
