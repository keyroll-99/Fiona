namespace Fiona.IDE.ProjectManager.Exceptions
{
    public class ProjectNotFoundException(string path) : Exception($"Project {path} not found")
    {
        
    }
}