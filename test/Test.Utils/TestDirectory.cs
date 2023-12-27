// This file is licensed to you under MIT license.

using IOPath = System.IO.Path;

namespace Test.Utils
{
    internal class TestDirectory : IDisposable
    {
        public const int NumberOfRetries = 5;
        public string Path { get; }
        public DirectoryInfo Info { get; }

        private TestDirectory(string path)
        {
            Path = path;
            Info = new DirectoryInfo(path);
        }

        public static TestDirectory Create()
        {
            int retries = 0;
            while (retries < NumberOfRetries)
            {
                string guid = Guid.NewGuid().ToString();

                string fullPath = IOPath.Combine(IOPath.GetTempPath(), guid);

                // Temporary folder with the same name already exists, retry.
                if (Directory.Exists(fullPath))
                {
                    retries++;
                    continue;
                }

                Directory.CreateDirectory(fullPath);
                return new(fullPath);
            }

            throw new TestDirectoryCreationException($"Failed to create new test directory. Retried {retries} time(s)");
        }

        public void Dispose() => Directory.Delete(Path, recursive: true);
    }
}
