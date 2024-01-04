// This file is licensed to you under MIT license.

using FluentValidation;
using FluentValidation.Results;
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

public class PlcProjectPackStrategy(ILogger? logger) : IPackStrategy
{
    private readonly ILogger? _logger = logger;

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

    /// <summary>
    /// Pack a TwinGet package when given a PLC project file.
    /// </summary>
    /// <param name="packCommand"></param>
    /// <returns>True if successul, otherwise false.</returns>
    private static async Task<bool> PackFromProjectFileAsync(IPackCommand packCommand)
    {
        string plcLibrary = string.Empty;

        try
        {
            plcLibrary = await SavePlcLibraryAsync(packCommand);
        }
        catch (PackagingException ex)
        {
            ex.LogWith(packCommand.Logger);
        }
        catch (Exception ex)
        {
            packCommand.Logger?.LogError(ex.Message);
        }

        if (string.IsNullOrEmpty(plcLibrary))
        {
            packCommand.Logger?.LogError(ErrorStrings.FailedToSavePlcLibrary, packCommand.Path);
            packCommand.Logger?.LogError(SuggestionStrings.MustSpecifySolutionFile);

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

    /// <summary>
    /// Build a TwinGet package from the given library file as artifact.
    /// </summary>
    /// <param name="packCommand"></param>
    /// <param name="libraryPath">The absolute path to the library file.</param>
    /// <returns>True if successful, otherwise false.</returns>
    /// <exception cref="FileNotFoundException"></exception>
    private static bool BuildPackage(IPackCommand packCommand, string libraryPath)
    {
        ArgumentException.ThrowIfNullOrEmpty(libraryPath, nameof(libraryPath));
        if (!File.Exists(libraryPath))
        {
            throw new FileNotFoundException(libraryPath);
        }

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

        packCommand.Logger?.LogInformation(OtherStrings.PackSuccess, outputPath);

        return true;
    }

    /// <summary>
    /// Save the PLC project as a library.
    /// </summary>
    /// <param name="packCommand"></param>
    /// <returns>The path to the library if successful. Otherwise <see cref="string.Empty"/>.</returns>
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

        // Begin AutomationInterface.
        var initAiTask = Task.Factory.StartNew(
            () =>
            {
                return new AutomationInterface();
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

        // Return if we cannot resovle the solution.
        if (string.IsNullOrEmpty(resolvedSolution))
        {
            packCommand.Logger?.LogError(
                ErrorStrings.FailedToResolveSolutionFile,
                packCommand.Path
            );
            packCommand.Logger?.LogError(
                ErrorStrings.FailedToResolveSolutionFile,
                packCommand.Path
            );
            return string.Empty;
        }

        // Begin saving PLC as library.
        var savePlcLibTask = initAiTask.ContinueWith(
            (prevTask) =>
            {
                using AutomationInterface ai = prevTask.Result;

                lock (libraryPathLock)
                {
                    try
                    {
                        // Suppress because we are in try block.
                        libraryPath =
                            ai.SavePlcProject(
                                packCommand.Path,
                                packCommand.OutputDirectory,
                                resolvedSolution
                            ) ?? string.Empty;
                    }
                    catch (PackagingException ex)
                    {
                        ex.LogWith(packCommand.Logger);
                    }
                    catch
                    {
                        throw;
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
        string result = await PlcProjectFileHelper
            .Create(packCommand.Path)
            .GetParentSolutionFileAsync();

        return result;
    }

    public IPackStrategy DoCustomValidation(IValidationContext validationContext)
    {
        _logger?.LogDebug($"Doing specific '{nameof(PlcProjectPackStrategy)}' validations.");
        // Throw if the validation object type is not correct.
        if (validationContext.InstanceToValidate.GetType() != typeof(PackCommand))
        {
            throw new PackagingException("Unexpected validation object type.");
        }

        var command = (PackCommand)validationContext.InstanceToValidate;

        if (!string.IsNullOrEmpty(command.Solution))
        {
            try
            {
                if (!Utils.PlcProjectBelongToSolution(command.Path, command.Solution))
                {
                    validationContext.RootContextData[PackCommand.CustomSolutionValidation] = new ValidationFailure(
                        nameof(IPackCommand.Solution),
                        string.Format(
                            ErrorStrings.SpecifiedInputFileDoesNotBelongToSolution,
                            command.Path,
                            command.Solution));
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.Message);
                return this;
            }
        }

        return this;
    }
}
