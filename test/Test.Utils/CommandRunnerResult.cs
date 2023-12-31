// This file is licensed to you under MIT license.

using System.Diagnostics;

namespace Test.Utils;

internal class CommandRunnerResult
{
    public Process Process { get; set; }

    public int ExitCode { get; set; }

    public string Output { get; set; }

    public string Errors { get; set; }

    public bool Success => ExitCode == 0;

    public string AllOuput => Output + Environment.NewLine + Errors;

    internal CommandRunnerResult(Process process, int exitCode, string output, string error)
    {
        Process = process;
        ExitCode = exitCode;
        Output = output;
        Errors = error;
    }
}
