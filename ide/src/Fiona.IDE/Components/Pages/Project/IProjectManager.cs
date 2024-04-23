using Fiona.IDE.Components.Pages.Project.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fiona.IDE.Components.Pages.Project
{
    public interface IProjectManager
    {
        bool IsLoaded();
        string? GetName();
        string GetPath();
        Task<string> CreateProject(string path, string name);
        Task LoadProject(string path);
        IEnumerable<ProjectFile>? GetFiles();
        Task CreateFileAsync(string name, string folderPath);

    }
}