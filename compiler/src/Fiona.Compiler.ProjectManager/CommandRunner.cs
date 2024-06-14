using Serilog;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Fiona.Compiler.ProjectManager;

internal class CommandRunner : ICommandRunner
{
    private string _shell;
    private readonly ILogger _logger;

    public CommandRunner(ILogger logger)
    {
        _logger = logger;
        _shell = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "powershell.exe" : "/bin/bash";
    }

    public async Task RunCommandAsync(string command, string? workingDirectory = null)
    {
        _logger.Information("Run command {Command}", command);
        string shellArgs = GetShellArgs(command);
        ProcessStartInfo processStartInfo = new()
        {
            FileName = _shell,
            Arguments = shellArgs,
            WorkingDirectory = workingDirectory,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        using Process process = new();
        process.StartInfo = processStartInfo;
        process.Start();
        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();
        
        await process.WaitForExitAsync();
        
        _logger.Information(output);
        if (!string.IsNullOrWhiteSpace(error))
        {
            _logger.Error(error);
        }
        
    }

    private static string GetShellArgs(string command)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return $"-Command \" {command}\"";
        }

        return $"-c \"{command}\"";
    }
    
    
}
