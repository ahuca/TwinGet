// This file is licensed to you under MIT license.

using System.Runtime.Versioning;
using TwinGet.TwincatInterface.Exceptions;
using TwinGet.TwincatInterface.ProjectFileUtils;
using TwinGet.TwincatInterface.Utils;
using static TwinGet.TwincatInterface.TwincatConstants;

namespace TwinGet.TwincatInterface
{
    [SupportedOSPlatform("windows")]
    public class AutomationInterface : IDisposable, IAutomationInterface
    {
        private bool _disposedValue;
        private readonly TwincatDteProvider _dteProvider;
        private EnvDTE80.DTE2 _dte
        {
            get => _dteProvider.Dte;
        }
        private EnvDTE.Solution? _solution;
        private EnvDTE.SolutionBuild? _solutionBuild;
        private readonly List<TwincatProject> _twincatProjects = [];

        public string ProgId
        {
            get => _dteProvider.ProgId;
        }
        public bool IsSolutionOpen
        {
            get => _solution?.IsOpen ?? false;
        }
        public string LoadedSolutionFile
        {
            get => _solution?.FileName ?? string.Empty;
        }
        public IReadOnlyList<ITwincatProject> TwincatProjects
        {
            get => _twincatProjects;
        }

        public AutomationInterface()
        {
            _dteProvider = new(this, true);
        }

        protected bool TryCleanUpDteProvider()
        {
            if (_dteProvider is null)
            {
                return false;
            }

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

        // TODO: if we throw everytime we fail to instantiate a DTE instance, is this method even needed?
        private void ThrowIfDteIsNull()
        {
            if (_dte is null)
            {
                throw new DteInstanceIsNullException(
                    $"No {nameof(EnvDTE80.DTE2)} instance available."
                );
            }
        }

        private static void ThrowSolutionPathNotFound(string solutionPath)
        {
            throw new FileNotFoundException(
                $"Provided solution path does not exists.",
                solutionPath
            );
        }

        public void LoadSolution(string filePath)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));
            if (!Path.Exists(filePath))
            {
                ThrowSolutionPathNotFound(filePath);
            }

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

        /// <summary>
        /// Save a PLC project as library.
        /// </summary>
        /// <param name="plcProjectPath">The path to the PLC project file.</param>
        /// <param name="outputDirectory">The output directory for the library.</param>
        /// <param name="solutionPath">The path to the solution file containing the PLC project.</param>
        /// <returns>The absolute path to the library file if successfully saved, otherwise, returns <see cref="string.Empty"/>.</returns>
        public string SavePlcProject(
            string plcProjectPath,
            string outputDirectory,
            string solutionPath = ""
        )
        {
            ArgumentException.ThrowIfNullOrEmpty(plcProjectPath, nameof(plcProjectPath));
            ArgumentException.ThrowIfNullOrEmpty(outputDirectory, nameof(outputDirectory));

            ThrowIfDteIsNull();

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            string fullPlcProjectPath = Path.GetFullPath(plcProjectPath);

            string resolvedSolutionPath = solutionPath;
            // If the given solution path is null or empty, we try to find the right solution.
            if (string.IsNullOrEmpty(solutionPath))
            {
                var helper = PlcProjectFileHelper.Create(plcProjectPath);
                resolvedSolutionPath = helper.GetParentSolutionFile();
            }

            // If the solution is not already loaded, we load it.
            if (
                !resolvedSolutionPath.Equals(LoadedSolutionFile, StringComparison.OrdinalIgnoreCase)
            )
            {
                LoadSolution(resolvedSolutionPath);
            }

            IPlcProject plcProjectToSave = GetPlcProjects()
                .Where(p =>
                {
                    return fullPlcProjectPath.Equals(
                        p.AbsolutePath,
                        StringComparison.OrdinalIgnoreCase
                    );
                })
                .First();

            if (plcProjectToSave is null)
            {
                return string.Empty;
            }

            string fileName = $"{plcProjectToSave.Title}{TwincatPlcLibraryExtension}";
            string fullPath = Path.Combine(outputDirectory, fileName);

            plcProjectToSave.SaveAsLibrary(fullPath, false);

            return fullPath;
        }

        public IEnumerable<IPlcProject> GetPlcProjects()
        {
            return TwincatProjects.SelectMany(t => t.PlcProjects);
        }
    }
}
