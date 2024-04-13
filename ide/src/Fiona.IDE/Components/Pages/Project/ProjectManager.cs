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

            FslnFile fslnFile = new(name, Enumerable.Empty<string>());
            await using Stream fslnFileStream = new MemoryStream();
            await JsonSerializer.SerializeAsync(fslnFileStream, fslnFile);
            await using FileStream fileStream = File.Create(fullPath);

            fslnFileStream.Seek(0, SeekOrigin.Begin);
            await fslnFileStream.CopyToAsync(fileStream);

            await _commandRunner.RunCommandAsync("dotnet new console", path);
            await _commandRunner.RunCommandAsync($"dotnet new sln --name {name}", path);
            await _commandRunner.RunCommandAsync("dotnet add package Fiona.Hosting", path);
            await _commandRunner.RunCommandAsync($"dotnet sln add {path}", path);

            return fullPath;
        }

        public async Task LoadProject(string path)
        {

            await using FileStream fs = new(path, FileMode.Open, FileAccess.Read);

            Project = await JsonSerializer.DeserializeAsync<FslnFile>(fs);
        }

        public bool IsLoaded()
            => Project is not null;
    }
}