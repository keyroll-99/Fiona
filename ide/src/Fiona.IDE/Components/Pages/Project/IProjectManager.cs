namespace Fiona.IDE.Components.Pages.Project
{
    public interface IProjectManager
    {
        Task<string> CreateProject(string path, string name);
        Task LoadProject(string path);
        bool IsLoaded();
    }
}