namespace Fiona.IDE.ProjectManager.Exceptions
{
    public class FileAlreadyExistsException(string path) : Exception($"File {path} already exists")
    {
        
    }
}