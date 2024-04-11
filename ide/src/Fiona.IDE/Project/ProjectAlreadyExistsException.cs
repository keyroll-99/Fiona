namespace Fiona.IDE.Project
{
    public class ProjectAlreadyExistsException(string? name) : Exception($"Project {name} already exists");

}