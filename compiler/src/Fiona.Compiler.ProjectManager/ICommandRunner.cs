using Serilog;

namespace Fiona.Compiler.ProjectManager;

public interface ICommandRunner
{
    Task RunCommandAsync(string command, string? workingDirectory = null);

    Task RunCommandInBackground(string command, string? workingDirectory = null);
}