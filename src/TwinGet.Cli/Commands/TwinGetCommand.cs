// This file is licensed to you under MIT license.

using System.CommandLine;

namespace TwinGet.Cli.Commands;

public class TwinGetCommand : RootCommand
{
    public TwinGetCommand()
        : base("TwinGet CLI. A package management for TwinCAT library.") { }
}
