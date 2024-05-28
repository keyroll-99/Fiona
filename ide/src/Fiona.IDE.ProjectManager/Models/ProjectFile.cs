using Fiona.IDE.ProjectManager.Exceptions;
using System.Text.Json.Serialization;

namespace Fiona.IDE.ProjectManager.Models
{
    public sealed class ProjectFile
    {
        public string Path { get; }
        public string Name { get; }
        public string Namespace { get; }
        public const string Extension = "fn";


        [JsonConstructor]
        private ProjectFile(string path)
        {
            Path = path;
            Name = path.Split(System.IO.Path.DirectorySeparatorChar).Last();
            Namespace = path.Replace(System.IO.Path.DirectorySeparatorChar.ToString(), ".").Split(":").Last().Replace(".fn", "")[1..];
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
    }
}