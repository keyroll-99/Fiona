using Fiona.IDE.ProjectManager.Exceptions;
using System.Text.Json.Serialization;

namespace Fiona.IDE.ProjectManager.Models
{
    public sealed class ProjectFile
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

        internal static ProjectFile Create(string path)
        {
            if (File.Exists(path))
            {
                throw new FileAlreadyExistsException(path);
            }
            File.Create(path);
            
            return new ProjectFile(path);
        }

        internal string GetContent()
        {
            return File.ReadAllText(Path);
        }
    }
}