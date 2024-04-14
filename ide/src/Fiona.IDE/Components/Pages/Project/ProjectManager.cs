using Fiona.IDE.Components.Pages.Project.Exceptions;
using Fiona.IDE.Components.Pages.Project.Models;
using System.Text.Json;

namespace Fiona.IDE.Components.Pages.Project
{
    internal class ProjectManager(ICommandRunner commandRunner) : IProjectManager
    {
        private readonly ICommandRunner _commandRunner = commandRunner;
        private FslnFile? Project { get; set; }

        public async Task<string> CreateProject(string path, string name)
        {
            string fullPath = $"{path}/{name}.fsln";
            if (File.Exists(fullPath))
            {
                throw new ProjectAlreadyExistsException(fullPath);
            }

            await CreateSln(path, name);
            await CreateFsln(name, fullPath);

            return fullPath;
        }

        public async Task LoadProject(string path)
        {
            string? filePath = Directory.GetFiles(path, "*.fsln").FirstOrDefault();
            if (filePath is null)
            {
                throw new ProjectNotFoundException(path);
            }
            await using FileStream fs = new(filePath, FileMode.Open, FileAccess.Read);

            Project = await JsonSerializer.DeserializeAsync<FslnFile>(fs);
        }

        public IEnumerable<ProjectFile>? GetFiles()
            => Project?.ProjectFileUrl;

        public bool IsLoaded()
            => Project is not null;

        private static async Task CreateFsln(string name, string fullPath)
        {
            FslnFile fslnFile = new(name, Enumerable.Empty<ProjectFile>());
            await using Stream fslnFileStream = new MemoryStream();
            await JsonSerializer.SerializeAsync(fslnFileStream, fslnFile);
            await using FileStream fileStream = File.Create(fullPath);

            fslnFileStream.Seek(0, SeekOrigin.Begin);
            await fslnFileStream.CopyToAsync(fileStream);
        }

        private async Task CreateSln(string path, string name)
        {
            await _commandRunner.RunCommandAsync("dotnet new console", path);
            await _commandRunner.RunCommandAsync($"dotnet new sln --name {name}", path);
            await _commandRunner.RunCommandAsync("dotnet add package Fiona.Hosting", path);
            await _commandRunner.RunCommandAsync($"dotnet sln add {path}", path);
        }
    }
}