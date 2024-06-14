namespace Fiona.Compiler.ProjectManager;

public interface ICommandRunner
{
    Task RunCommandAsync(string command, string? workingDirectory = null);
}