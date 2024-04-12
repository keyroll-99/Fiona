using System.Text.Json.Serialization;

namespace Fiona.IDE.Components.Pages.Project.Models
{
    internal class FslnFile
    {
        public FslnFile()
        {
        }

        public FslnFile(string name, IEnumerable<string> projectFileUrl)
        {
            Name = name;
            ProjectFileUrl = projectFileUrl.ToList();
        }

        public string Name { get; private set; }
        public List<string> ProjectFileUrl { get; private set;}
    }
}