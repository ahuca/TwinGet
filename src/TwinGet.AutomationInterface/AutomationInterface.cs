// This file is licensed to you under MIT license.

using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using EnvDTE80;
using TwinGet.AutomationInterface.ComMessageFilter;
using TwinGet.AutomationInterface.Exceptions;
using TwinGet.AutomationInterface.Utils;
using static TwinGet.AutomationInterface.AutomationInterfaceConstants;

namespace TwinGet.AutomationInterface
{
    [SupportedOSPlatform("windows")]
    public class AutomationInterface : IDisposable
    {
        private string _progId;
        private EnvDTE.Solution? _solution;
        private EnvDTE.SolutionBuild? _solutionBuild;
        private readonly EnvDTE80.DTE2? _dte;
        private bool _disposedValue;
        private readonly List<TwincatProject> _twincatProjects = new();

        public string ProgId { get => _progId; }
        public bool IsSolutionOpen { get => _solution?.IsOpen ?? false; }
        public string LoadedSolutionFile { get => _solution?.FileName ?? string.Empty; }
        public IReadOnlyList<TwincatProject> TwincatProjects { get => _twincatProjects; }

        public AutomationInterface()
        {
            MessageFilter.Register();
            _dte = TryInitializeDte(out _progId);
            ThrowIfFailToInitializeDte(_dte);
        }

        internal AutomationInterface(ref EnvDTE80.DTE2? dte)
        {
            MessageFilter.Register();
            _dte = TryInitializeDte(out _progId);
            dte = _dte;
            if (_dte is null) { throw new CouldNotCreateTwincatDteException("Is TwinCAT installed in this system?"); }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing) { }

                CleanUp();
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

        private void CleanUp()
        {
            _progId = string.Empty;

            if (_dte is not null)
            {
                _dte.Quit();
                Marshal.ReleaseComObject(_dte);
                MessageFilter.Revoke();
            }
        }

        private void ThrowIfFailToInitializeDte(EnvDTE80.DTE2? dte)
        {
            if (dte is null)
            {
                throw new CouldNotCreateTwincatDteException($"Failed to create a DTE instance due to missing TwinCAT XAE or TwinCAT-intergrated Visual Studio installation. TwinCAT can be downloaded from: {AutomationInterfaceConstants.TwincatXaeDownloadUrl}");
            }
        }

        /// <summary>
        /// Try to create a Visual Studio (or TwinCAT XAE) DTE instance.
        /// </summary>
        /// <param name="progId">The ProgId of the created instance if successful, empty string if not.</param>
        /// <returns>The created DTE instance if successful, null if not.</returns>
        private static EnvDTE80.DTE2? TryInitializeDte(out string progId)
        {
            foreach (string p in AutomationInterfaceConstants.SupportedProgIds)
            {
                Type? t = Type.GetTypeFromProgID(p);

                if (t is null) { continue; }

                EnvDTE80.DTE2? dte;
                try
                {
                    dte = (EnvDTE80.DTE2?)Activator.CreateInstance(t);
                    if (dte is null) { continue; }
                }
                catch { continue; }

                if (dte.IsTwinCatIntegrated())
                {
                    progId = p;
                    return dte;
                }
            }

            progId = string.Empty;
            return null;
        }

        private void ThrowIfDteIsNull()
        {
            if (_dte is null) { throw new DteInstanceIsNullException($"No {nameof(DTE2)} instance available."); }
        }

        private static void ThrowNullOrEmptySolutionPath()
        {
            throw new ArgumentException("Solution path cannot be null or empty.");
        }

        private static void ThrowSolutionPathNotFound(string solutionPath)
        {
            throw new FileNotFoundException($"Provided solution path \"{solutionPath}\" does not exists.");
        }

        private static void ThrowIfInvalidSolutionPath(string solutionPath)
        {
            if (string.IsNullOrEmpty(solutionPath)) { ThrowNullOrEmptySolutionPath(); }
            if (!Path.Exists(solutionPath)) { ThrowSolutionPathNotFound(solutionPath); }
        }

        public void LoadSolution(string filePath)
        {
            ThrowIfInvalidSolutionPath(filePath);
            ThrowIfDteIsNull();

            filePath = Path.GetFullPath(filePath);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            _solution = _dte.Solution;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
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

        public static void SaveProjectAsLibrary(string outFile, string solutionPath = "")
        {

        }
    }
}
