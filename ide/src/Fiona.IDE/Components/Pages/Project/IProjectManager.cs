using Fiona.IDE.Components.Pages.Project.Models;

namespace Fiona.IDE.Components.Pages.Project
{
    public interface IProjectManager
    {
        Task<string> CreateProject(string path, string name);
        Task LoadProject(string path);
        IEnumerable<ProjectFile>? GetFiles();
        bool IsLoaded();
    }
}