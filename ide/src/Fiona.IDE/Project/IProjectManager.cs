namespace Fiona.IDE.Project
{
    public interface IProjectManager
    {
        Task<string> CreateProject(string path, string name);
        Task LoadProject(string path);
    }
}