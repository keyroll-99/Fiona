namespace Fiona.IDE.Project.Models
{
    public class FslnFile(string name, IEnumerable<string> projectFileUrl)
    {
        public string Name { get; } = name;
        public List<string> ProjectFileUrl { get; } = projectFileUrl.ToList();
    }
}