// This file is licensed to you under MIT license.

using Microsoft.Extensions.Logging;
using NuGet.Packaging;
using NuGet.Versioning;
using TwinGet.Core.Commands;
using TwinGet.TwincatInterface;
using TwinGet.TwincatInterface.Dto;
using TwinGet.TwincatInterface.Utils;
using static NuGet.Configuration.NuGetConstants;

namespace TwinGet.Core.Packaging
{
    public class PackageService : IPackageService
    {
        public PackageService() { }
        public bool Pack(IPackCommand packCommand)
        {
            return PackAsync(packCommand).Result;
        }

        /// <summary>
        /// Package a PLC project into a NuGet package.
        /// </summary>
        /// <param name="packCommand"></param>
        /// <returns>True if successful, otherwise false.</returns>
        public async Task<bool> PackAsync(IPackCommand packCommand)
        {
            ArgumentException.ThrowIfNullOrEmpty(packCommand.Path, nameof(packCommand.Path));
            ArgumentException.ThrowIfNullOrEmpty(packCommand.OutputDirectory, nameof(packCommand.OutputDirectory));

            if (TwincatUtils.IsPlcProjectFileExtension(packCommand.Path))
            {
                return await PackFromProjectFileAsync(packCommand);
            }
            else if (Utils.IsNuspecExtension(packCommand.Path))
            {
                return await PackFromNuspecFileAsync(packCommand);
            }

            return true;
        }

        private Task<bool> PackFromNuspecFileAsync(IPackCommand packCommand) => throw new NotImplementedException();

        private static async Task<bool> PackFromProjectFileAsync(IPackCommand packCommand)
        {

            string? plcProject = null;

            Task<string>? getSolutionTask = null;
            if (string.IsNullOrEmpty(packCommand.Solution))
            {
                getSolutionTask = GetParentSolutionFileAsync(packCommand.Path);
            }

            var thread = new Thread(async () =>
            {
                using AutomationInterface ai = new();

                if (string.IsNullOrEmpty(packCommand.Solution))
                {
                    packCommand.Solution = await getSolutionTask;
                }

                try
                {
                    plcProject = ai.SavePlcProject(packCommand.Path, packCommand.OutputDirectory, packCommand.Solution);
                }
                catch (Exception ex)
                {
                    packCommand.Logger?.LogError(ex.Message);
                }

            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            if (string.IsNullOrEmpty(plcProject))
            {
                throw new PackagingException($"Failed to save the {packCommand.Path} as library.");
            }

            var plcProjectData = TwincatUtils.DeserializeXmlFileToProjectData<PlcProjectData>(packCommand.Path);

            ManifestMetadata metadata = new()
            {
                Authors = new List<string>() { plcProjectData.PropertyGroup.Author ?? string.Empty },
                Version = new NuGetVersion(plcProjectData.PropertyGroup.ProjectVersion ?? string.Empty),
                Id = plcProjectData.PropertyGroup.Title,
                Description = plcProjectData.PropertyGroup.Description,
            };

            var files = new List<ManifestFile>()
            {
                new() { Source = plcProject, Target = "lib" }
            };

            if (!Directory.Exists(packCommand.OutputDirectory))
            {
                Directory.CreateDirectory(packCommand.OutputDirectory);
            }

            var packageBuilder = new PackageBuilder();
            packageBuilder.Populate(metadata);
            packageBuilder.PopulateFiles(string.Empty, files);

            string packageFile = Path.Combine(packCommand.OutputDirectory, $"{packageBuilder.Id}{PackageExtension}");
            using FileStream stream = File.Open(packageFile, FileMode.OpenOrCreate);
            packageBuilder.Save(stream);

            return true;
        }

        private static async Task<string> GetParentSolutionFileAsync(string plcProjectPath)
        {
            var tcProjectPath = await TwincatUtils.GetParentTwincatProjectFileAsync(plcProjectPath);
            return await TwincatUtils.GetParentSolutionFileAsync(tcProjectPath);
        }
    }
}
