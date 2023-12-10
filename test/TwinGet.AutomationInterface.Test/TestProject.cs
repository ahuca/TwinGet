// This file is licensed to you under MIT license.

using Microsoft.Build.Construction;

namespace TwinGet.AutomationInterface.Test
{
    internal class TestProject : IDisposable
    {
        private bool _disposedValue;
        public string Path { get; private set; }
        public string SolutionPath { get; }
        public SolutionFile SolutionFile { get; }

        public TestProject()
        {
            Path = System.IO.Path.Join(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(Path);

            TwinGet.Utils.IO.Directory.CopyDirectory(
    TestTwincatProjectConstants.s_testTwincatProject,
    Path, FileShare.Read).Wait();

            SolutionPath = Directory.GetFiles(Path, "*.sln", SearchOption.AllDirectories).First();

            SolutionFile = SolutionFile.Parse(SolutionPath);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing) { }

                if (Directory.Exists(Path)) { Directory.Delete(Path, true); }
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
