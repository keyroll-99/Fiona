using Fiona.IDE.Components.Pages.Project.Exceptions;
using Fiona.IDE.Components.Pages.Project.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Fiona.IDE.Components.Pages.Project
{
    internal class ProjectManager(ICommandRunner commandRunner) : IProjectManager
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

        public IEnumerable<ProjectFile>? GetFiles()
            => Project?.ProjectFileUrl;

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