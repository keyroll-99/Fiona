namespace Fiona.IDE.Components.Pages.Project.Exceptions
{
    public class FileAlreadyExistsException(string path) : Exception($"File {path} already exists")
    {
        
    }
}