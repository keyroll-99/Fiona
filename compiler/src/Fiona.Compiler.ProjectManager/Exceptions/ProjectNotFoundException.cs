namespace Fiona.Compiler.ProjectManager.Exceptions;

public sealed class ProjectNotFoundException(string path) : Exception($"Project {path} not found")
{

}