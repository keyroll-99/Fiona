using Fiona.IDE.ProjectManager.Exceptions;
using Fiona.IDE.ProjectManager.Models;

namespace Fiona.IDE.ProjectManager
{
    internal sealed class ProjectManager(ICommandRunner commandRunner) : IProjectManager
    {
        private FslnFile? Project { get; set; }

        public string GetPath()
        {
            if (Project is null)
            {
                throw new Exception("Project not loaded");
            }

            return Project.Path;
        }

        public async Task<string> CreateProject(string path, string name)
        {
            string fullPath = $"{path}{Path.DirectorySeparatorChar}{name}.fsln";
            if (File.Exists(fullPath))
            {
                throw new ProjectAlreadyExistsException(fullPath);
            }

            await CreateSln(path, name);
            await CreateFsln(name, path);
            return fullPath;
        }

        public async Task LoadProject(string path)
        {

            Project = await FslnFile.LoadAsync(path);
        }

        public IEnumerable<ProjectFile> GetFiles()
            => Project?.ProjectFiles ?? Enumerable.Empty<ProjectFile>() ;
        public ProjectFile GetProjectFileByNamespace(string @namespace) 
            => Project?.ProjectFiles!.FirstOrDefault(x => x.Class.Namespace == @namespace) ?? throw new ProjectFileNotFoundException(@namespace);

        public Task CreateFileAsync(string name, string folderPath)
        {
            return Project!.AddFile(name, folderPath);
        }

        public bool IsLoaded()
            => Project is not null;

        public string? GetName()
            => Project?.Name;

        private async Task CreateFsln(string name, string pathToFolder)
        {
            Project = await FslnFile.CreateAsync(name, pathToFolder);
        }

        private async Task CreateSln(string path, string name)
        {
            await commandRunner.RunCommandAsync("dotnet new console", path);
            await commandRunner.RunCommandAsync($"dotnet new sln --name {name}", path);
            await commandRunner.RunCommandAsync("dotnet add package Fiona.Hosting", path);
            await commandRunner.RunCommandAsync($"dotnet sln add {path}", path);
        }
    }
}