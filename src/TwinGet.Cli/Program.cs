// This file is licensed to you under MIT license.

using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using Microsoft.Extensions.Hosting;
using TwinGet.Cli;
using TwinGet.Cli.Commands;
using TwinGet.Core;

class Program
{
    static async Task<int> Main(string[] args)
    {
        Parser runner = BuildCommandLine()
            .UseHost(_ => Host.CreateDefaultBuilder(args)
            , (builder) => builder
            .ConfigureServices((hostContext, services) =>
            {
                services.AddCore();
                services.AddLogger();
            })
            .UseCommandHandler<PackCommand, PackCommand.Handler>())
            .UseDefaults()
            .Build();

        return await runner.InvokeAsync(args);
    }

    static CommandLineBuilder BuildCommandLine()
    {
        var root = new TwinGetCommand();
        root.AddCommand(new PackCommand());

        return new CommandLineBuilder(root);
    }
}
