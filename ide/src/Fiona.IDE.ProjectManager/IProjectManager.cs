using Fiona.IDE.ProjectManager.Models;

namespace Fiona.IDE.ProjectManager
{
    public interface IProjectManager
    {
        bool IsLoaded();
        string? GetName();
        string GetPath();
        Task<string> CreateProject(string path, string name);
        Task LoadProject(string path);
        IEnumerable<ProjectFile> GetFiles();
        ProjectFile GetProjectFileByNamespaceAndName(string @namespace, string name);
        Task CreateFileAsync(string name, string folderPath);

    }
}