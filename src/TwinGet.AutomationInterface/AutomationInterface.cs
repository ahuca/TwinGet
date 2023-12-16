// This file is licensed to you under MIT license.

using System.Runtime.Versioning;
using TwinGet.AutomationInterface.Exceptions;
using TwinGet.AutomationInterface.Utils;
using static TwinGet.AutomationInterface.TwincatConstants;

namespace TwinGet.AutomationInterface
{
    [SupportedOSPlatform("windows")]
    public class AutomationInterface : IDisposable, IAutomationInterface
    {
        private bool _disposedValue;
        private readonly TwincatDteProvider _dteProvider;
        private EnvDTE80.DTE2 _dte { get => _dteProvider.Dte; }
        private EnvDTE.Solution? _solution;
        private EnvDTE.SolutionBuild? _solutionBuild;
        private readonly List<TwincatProject> _twincatProjects = new();

        public string ProgId { get => _dteProvider.ProgId; }
        public bool IsSolutionOpen { get => _solution?.IsOpen ?? false; }
        public string LoadedSolutionFile { get => _solution?.FileName ?? string.Empty; }
        public IReadOnlyList<ITwincatProject> TwincatProjects { get => _twincatProjects; }

        public AutomationInterface()
        {
            _dteProvider = new(this, true);
        }

        protected bool TryCleanUpDteProvider()
        {
            if (_dteProvider is null) { return false; }

            /// We only dispose the <see cref="_dteProvider"/> that we created.
            if (_dteProvider.Owner == this)
            {
                _dteProvider.Dispose();
                return true;
            }

            return false;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing) { }

                TryCleanUpDteProvider();
                _disposedValue = true;
            }
        }

        ~AutomationInterface()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private void ThrowIfDteIsNull()
        {
            if (_dte is null) { throw new DteInstanceIsNullException($"No {nameof(EnvDTE80.DTE2)} instance available."); }
        }

        private static void ThrowSolutionPathNotFound(string solutionPath)
        {
            throw new FileNotFoundException($"Provided solution path does not exists.", solutionPath);
        }

        public void LoadSolution(string filePath)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));
            if (!Path.Exists(filePath)) { ThrowSolutionPathNotFound(filePath); }

            ThrowIfDteIsNull();

            filePath = Path.GetFullPath(filePath);
            _solution = _dte.Solution;
            _solutionBuild = _dte.Solution.SolutionBuild;
            _solution.Open(filePath);

            // Get TwinCAT projects
            for (int i = ProjectItemStartingIndex; i <= _dte.Solution.Projects.Count; i++)
            {
                EnvDTE.Project currentItem = _dte.Solution.Projects.Item(i);

                if (currentItem.IsTwincatProject())
                {
                    _twincatProjects.Add(new TwincatProject(currentItem));
                }
            }
        }

        public static void SaveProjectAsLibrary(string plcProjectName, string outFile, string solutionPath = "")
        {

        }
    }
}
