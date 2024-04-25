namespace Fiona.IDE.ProjectManager.Exceptions
{
    public sealed class FileAlreadyExistsException(string path) : Exception($"File {path} already exists")
    {
        
    }
}