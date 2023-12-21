// This file is licensed to you under MIT license.

using TwinGet.Core.Commands;
using TwinGet.TwincatInterface;

namespace TwinGet.Core.Packaging
{
    public class PackageService : IPackageService
    {
        public PackageService() { }
        public bool Pack(IPackCommand packCommand) => throw new NotImplementedException();
        public async Task<bool> PackAsync(IPackCommand packCommand)
        {
            IPlcProject? plcLibrary = await GenerateLibraryFile(packCommand);

            if (plcLibrary is null)
            {
                throw new PackagingException($"Failed to save the {packCommand.Path} as library.");
            }

            // TODO continue packing the library file.
            //PackageBuilder packageBuilder = new();

            return true;
        }

        private async Task<IPlcProject?> GenerateLibraryFile(IPackCommand packCommand)
        {
            IPlcProject? plcProject = null;
            var thread = new Thread(() =>
            {
                using AutomationInterface ai = new();

                plcProject = ai.SavePlcProject(packCommand.Path, packCommand.OutputDirectory, packCommand.Solution);
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            return plcProject;
        }
    }
}
