// This file is licensed to you under MIT license.

using Microsoft.Build.Construction;
using static TwinGet.AutomationInterface.AutomationInterfaceConstants;

namespace TwinGet.AutomationInterface.Test.TestUtils
{
    internal class TestProject : IDisposable
    {
        private bool _disposedValue;
        private readonly List<TestTwincatProject> _twincatProjects = [];
        public string RootPath { get; private set; }
        public string SolutionPath { get; }
        public SolutionFile SolutionFile { get; }
        public IReadOnlyList<TestTwincatProject> TwincatProjects { get => _twincatProjects; }

        public TestProject()
        {
            RootPath = System.IO.Path.Join(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(RootPath);

            TwinGet.Utils.IO.Directory.CopyDirectory(
    TestTwincatProjectConstants.s_testTwincatProject,
    RootPath, FileShare.Read).Wait();

            SolutionPath = Directory.GetFiles(RootPath, "*.sln", SearchOption.AllDirectories).First();

            SolutionFile = SolutionFile.Parse(SolutionPath);

            static bool isTwincatProject(string absolutePath) => TwincatProjectExtensions.Contains(System.IO.Path.GetExtension(absolutePath));

            _twincatProjects = new(SolutionFile.ProjectsInOrder
                .Where(x => isTwincatProject(x.AbsolutePath))
                .Select(x => new TestTwincatProject(x.ProjectName, x.AbsolutePath)));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing) { }

                if (Directory.Exists(RootPath)) { Directory.Delete(RootPath, true); }
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
