// This file is licensed to you under MIT license.

using TCatSysManagerLib;

namespace TwinGet.AutomationInterface
{
    public interface IPlcProject : ITcPlcIECProject3
    {
        public string Name { get; }
        public string? Company { get; }
        public string? Title { get; }
        public string? ProjectVersion { get; }
        public bool IsManagedLibrary
        {
            get
            {
                return !string.IsNullOrEmpty(Company) && !string.IsNullOrEmpty(Title) && !string.IsNullOrEmpty(ProjectVersion);
            }
        }
        public string FilePath { get; }
    }
}
