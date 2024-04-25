namespace Fiona.IDE.ProjectManager.Exceptions
{
    public sealed class ProjectAlreadyExistsException(string? name) : Exception($"Project {name} already exists");

}