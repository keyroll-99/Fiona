namespace Fiona.IDE.Components.Pages.Project
{
    internal class ProjectAlreadyExistsException(string? name) : Exception($"Project {name} already exists");

}