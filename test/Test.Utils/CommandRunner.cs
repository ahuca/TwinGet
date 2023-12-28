// This file is licensed to you under MIT license.

using System.Diagnostics;
using System.Text;
using Xunit.Abstractions;

namespace Test.Utils;

internal class CommandRunner(ITestOutputHelper output)
{
    public const int DefaultTimeoutInMilliseconds = 300000;

    private readonly ITestOutputHelper _output = output;

    public CommandRunnerResult Run(
        string exePath,
        string workingDirectory,
        string arguments,
        int timeOutInMilliseconds = DefaultTimeoutInMilliseconds,
        Action<StreamWriter>? inputAction = null)
    {
        StringBuilder output = new();
        StringBuilder error = new();

        using var process = new Process()
        {
            EnableRaisingEvents = true,
            StartInfo = new ProcessStartInfo(
                Path.GetFullPath(exePath, Path.GetFullPath(workingDirectory)),
                arguments)
            {
                WorkingDirectory = Path.GetFullPath(workingDirectory),
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = inputAction is not null,
            }
        };

        process.OutputDataReceived += (_, e) =>
        {
            if (e.Data != null)
            {
                output.AppendLine(e.Data);
            }
        };

        process.ErrorDataReceived += (_, e) =>
        {
            if (e.Data != null)
            {
                error.Append(e.Data);
            }
        };

        process.Start();

        inputAction?.Invoke(process.StandardInput);

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        bool processExited = process.WaitForExit(timeOutInMilliseconds);

        if (processExited)
        {
            return new CommandRunnerResult(
                process,
                process.ExitCode,
                output.ToString(),
                error.ToString());
        }

        try
        {
            process.Kill();
        }
        catch (Exception ex)
        {
            _output?.WriteLine(ex.Message);
            _output?.WriteLine(ex.HelpLink);
            _output?.WriteLine(ex.Source);
            _output?.WriteLine(ex.StackTrace);
        }

        throw new TimeoutException($"{process.StartInfo.FileName} {process.StartInfo.Arguments} timed out after {TimeSpan.FromMilliseconds(timeOutInMilliseconds).TotalSeconds:N0} seconds:{Environment.NewLine}Output:{output}{Environment.NewLine}Error:{error}");

    }
}
