// This file is licensed to you under MIT license.

using System.CommandLine;
using System.CommandLine.Invocation;
using MediatR;
using Microsoft.Extensions.Logging;
using TwinGet.Core.Commands;
using TwinGet.Core.Packaging;
using static NuGet.Configuration.NuGetConstants;
using static TwinGet.TwincatInterface.TwincatConstants;

namespace TwinGet.Cli.Commands;

public class PackCommand : Command
{
    public PackCommand() : base("pack", $"Pack a managed PLC project into a {PackageExtension}")
    {
        var fileArgument = new Argument<string>(
            name: "path",
            description: "The path to the PLC project file.",
            getDefaultValue: () => string.Empty);

        var solutionOption = new Option<string>(
            name: "--solution",
            description: $"The solution to which the \"{PlcProjectExtension}\" belongs to.",
            getDefaultValue: () => string.Empty);

        var outputDirectory = new Option<string>(
            name: "--output-directory",
            description: "Specifies the folder in which the created packages is stored. If no folder is specfified, the current folder is used.",
            getDefaultValue: () => string.Empty);

        AddArgument(fileArgument);
        AddOption(solutionOption);
        AddOption(outputDirectory);
    }

    public new class Handler : ICommandHandler, IPackCommand
    {
        private readonly IMediator _mediator;

        public string Path { get; set; }
        public string Solution { get; set; }
        public string OutputDirectory { get; set; }
        public ILogger Logger { get; set; }

        public Handler(IMediator mediator, ILogger logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            Logger = logger;
        }

        public int Invoke(InvocationContext context) => throw new NotImplementedException();
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0003:Remove qualification", Justification = "For clarity.")]
        public async Task<int> InvokeAsync(InvocationContext context)
        {
            try
            {
                await _mediator.Send(new Core.Commands.PackCommand
                {
                    Logger = this.Logger,
                    Path = this.Path,
                    Solution = this.Solution,
                    OutputDirectory = this.OutputDirectory
                });
            }
            catch (PackagingException ex)
            {
                Logger?.LogError(ex.AsLogMessage());
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex.Message);
            }

            return 0;
        }
    }
}
