namespace Fiona.IDE.Components.Pages.Project.Models
{
    public class FslnFile
    {
// #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
//         public FslnFile()
// #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
//         {
//         }

        public FslnFile(string name, List<ProjectFile> projectFileUrl, string path)
        {
            Name = name;
            Path = path;
            ProjectFileUrl = projectFileUrl;
        }

        public string Name { get; private set; }
        public string Path { get; private set; }
        public List<ProjectFile>? ProjectFileUrl { get; private set;}
    }
}