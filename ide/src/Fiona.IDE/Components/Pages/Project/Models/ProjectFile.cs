using Fiona.IDE.Components.Pages.Project.Exceptions;
using System.Text.Json.Serialization;

namespace Fiona.IDE.Components.Pages.Project.Models
{
    public class ProjectFile
    {
        public string Path { get; }
        public string Name { get; }

        public static string Extension = "fn";

        [JsonConstructor]
        private ProjectFile(string path)
        {
            Path = path;
            Name = path.Split(System.IO.Path.DirectorySeparatorChar).Last();

        }

        public static ProjectFile Create(string path)
        {
            if (File.Exists(path))
            {
                throw new FileAlreadyExistsException(path);
            }
            File.Create(path);
            
            return new ProjectFile(path);
        }

        public string GetContent()
        {
            return File.ReadAllText(Path);
        }
    }
}