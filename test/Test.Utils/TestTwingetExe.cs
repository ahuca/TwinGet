// This file is licensed to you under MIT license.

using IOPath = System.IO.Path;

namespace Test.Utils
{
    internal class TestTwingetExe
    {
        public string Path { get; } = string.Empty;

        public TestTwingetExe()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string exeName = "TwinGet.exe";

            Path = IOPath.Combine(baseDir, exeName);
        }
    }
}
