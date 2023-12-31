// This file is licensed to you under MIT license.

using Microsoft.Extensions.Logging;
using NuGet.Packaging;
using NuGet.Versioning;
using TwinGet.Core.Commands;
using TwinGet.TwincatInterface;
using TwinGet.TwincatInterface.Dto;
using TwinGet.TwincatInterface.ProjectFileUtils;
using TwinGet.TwincatInterface.Utils;
using TwinGet.Utils.Threading.Tasks;
using static NuGet.Configuration.NuGetConstants;
using Task = System.Threading.Tasks.Task;

namespace TwinGet.Core.Packaging;

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
        ArgumentException.ThrowIfNullOrEmpty(
            packCommand.OutputDirectory,
            nameof(packCommand.OutputDirectory)
        );

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

    private Task<bool> PackFromNuspecFileAsync(IPackCommand packCommand) =>
        throw new NotImplementedException();

    private static async Task<bool> PackFromProjectFileAsync(IPackCommand packCommand)
    {
        string plcLibrary = string.Empty;

        try
        {
            plcLibrary = await SavePlcLibraryAsync(packCommand);
        }
        catch (PackagingException ex)
        {
            packCommand.Logger?.LogError(ex.AsLogMessage());
        }
        catch (Exception ex)
        {
            packCommand.Logger?.LogError(ex.Message);
        }

        if (string.IsNullOrEmpty(plcLibrary))
        {
            packCommand.Logger?.LogError(PackagingErrors.FailedToSavePlcLibrary, packCommand.Path);
            return false;
        }

        bool result = BuildPackage(packCommand, plcLibrary);

        // Delete the library once we are done packing.
        if (!string.IsNullOrEmpty(plcLibrary))
        {
            File.Delete(plcLibrary);
        }

        return result;
    }

    private static bool BuildPackage(IPackCommand packCommand, string libraryPath)
    {
        ArgumentException.ThrowIfNullOrEmpty(libraryPath, nameof(libraryPath));

        var plcProjectData = TwincatUtils.DeserializeXmlFileToProjectData<PlcProjectData>(
            packCommand.Path
        );

        ManifestMetadata metadata =
            new()
            {
                Authors = new List<string>()
                {
                    plcProjectData.PropertyGroup.Author ?? string.Empty
                },
                Version = new NuGetVersion(
                    plcProjectData.PropertyGroup.ProjectVersion ?? string.Empty
                ),
                Id = plcProjectData.PropertyGroup.Title,
                Description = plcProjectData.PropertyGroup.Description,
            };

        var files = new List<ManifestFile>()
        {
            new() { Source = libraryPath, Target = "lib" }
        };

        if (!Directory.Exists(packCommand.OutputDirectory))
        {
            Directory.CreateDirectory(packCommand.OutputDirectory);
        }

        var packageBuilder = new PackageBuilder();
        packageBuilder.Populate(metadata);
        packageBuilder.PopulateFiles(string.Empty, files);

        string outputPath = Path.Combine(
            packCommand.OutputDirectory,
            $"{packageBuilder.Id}{PackageExtension}"
        );
        using FileStream stream = File.Open(outputPath, FileMode.OpenOrCreate);
        packageBuilder.Save(stream);

        packCommand.Logger?.LogInformation(PackagingStrings.PackSuccess, outputPath);

        return true;
    }

    private static async Task<string> SavePlcLibraryAsync(IPackCommand packCommand)
    {
        object libraryPathLock = new();
        string libraryPath = string.Empty;

        // Begin resolving solution if needed.
        Task<string>? getSolutionTask = null;
        if (string.IsNullOrEmpty(packCommand.Solution))
        {
            getSolutionTask = GetParentSolutionFileAsync(packCommand);
        }

        StaTaskScheduler staTaskScheduler = new(1);

        using var ai = new ThreadLocal<AutomationInterface>(() => new AutomationInterface());

        // Begin AutomationInterface.
        var initAiTask = Task.Factory.StartNew(
            () =>
            {
                var _ = ai.Value; // We do this so that ThreadLocal inititalize AutomationInterface();
            },
            CancellationToken.None,
            TaskCreationOptions.None,
            staTaskScheduler
        );

        string resolvedSolution;
        if (getSolutionTask is not null)
        {
            resolvedSolution = await getSolutionTask;
        }
        else
        {
            resolvedSolution = packCommand.Solution;
        }

        // Begin saving PLC as library.
        var savePlcLibTask = initAiTask.ContinueWith(
            (prevTask) =>
            {
                prevTask.Wait();

                lock (libraryPathLock)
                {
                    try
                    {
                        // Suppress because we are in try block.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        libraryPath =
                            ai.Value.SavePlcProject(
                                packCommand.Path,
                                packCommand.OutputDirectory,
                                resolvedSolution
                            ) ?? string.Empty;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    }
                    catch (Exception ex)
                    {
                        // Handle the custom exception.
                        if (ex is PackagingException packagingException)
                        {
                            packCommand.Logger?.LogError(packagingException.AsLogMessage());
                            if (!string.IsNullOrEmpty(packagingException.Source))
                            {
                                packCommand.Logger?.LogError(packagingException.Source);
                            }
                            if (!string.IsNullOrEmpty(packagingException.HelpLink))
                            {
                                packCommand.Logger?.LogError(packagingException.HelpLink);
                            }
                            packCommand.Logger?.LogError(packagingException.StackTrace);
                        }
                        // Rethrow any other exception.
                        else
                        {
                            throw;
                        }
                    }
                }
            },
            CancellationToken.None,
            TaskContinuationOptions.None,
            staTaskScheduler
        );

        await savePlcLibTask;

        return libraryPath;
    }

    private static async Task<string> GetParentSolutionFileAsync(IPackCommand packCommand)
    {
        string? result = await PlcProjectFileHelper
            .Create(packCommand.Path)
            .GetParentSolutionFileAsync();
        if (string.IsNullOrEmpty(result))
        {
            throw new PackagingException(PackagingErrors.FailedToResolveSolutionFile);
        }

        return result;
    }
}
