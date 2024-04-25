namespace Fiona.IDE.ProjectManager.Exceptions
{
    public class ProjectAlreadyExistsException(string? name) : Exception($"Project {name} already exists");

}