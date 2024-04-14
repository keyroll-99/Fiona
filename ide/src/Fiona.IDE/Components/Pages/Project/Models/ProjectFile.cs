namespace Fiona.IDE.Components.Pages.Project.Models
{
    public class ProjectFile(string path)
    {
        public string Path { get; } = path;
        public string Name { get; } = path.Split(System.IO.Path.DirectorySeparatorChar).Last();
    }
}