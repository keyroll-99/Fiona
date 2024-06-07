using Fiona.IDE.ProjectManager.Exceptions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Fiona.IDE.ProjectManager.Models;

public sealed class ProjectFile
{
    public string Path { get; }
    public string Name { get; }
    [NotMapped]
    public Class? Class { get; private set; }
    public const string Extension = "fn";

    private ProjectFile(string path, string name, Class? @class)
    {
        Path = path;
        Name = name;
        Class = @class;
    }

    internal static async Task<ProjectFile> LoadAsync(string path)
    {
        Class classInstance = await Class.Load(path);
        string name = path.Split(System.IO.Path.DirectorySeparatorChar).Last();
        return new ProjectFile(path, name, classInstance);
    }

    internal static async Task<ProjectFile> CreateAsync(string path)
    {
        if (File.Exists(path))
        {
            throw new FileAlreadyExistsException(path);
        }

        await using (FileStream fileHandler = File.Create(path))
        {
        }

        // Create a dummy project file to initialize the content
        ProjectFile projectFile = new(path, path.Split(System.IO.Path.DirectorySeparatorChar).Last(), null);
        await projectFile.SaveContentAsync(projectFile.GetBaseContent());

        return await LoadAsync(path);
    }

    private async Task SaveContentAsync(string content)
    {
        await using StreamWriter writer = new(Path);
        await writer.WriteAsync(content);
    }
}

internal static class ProjectFileExtensions
{
    public static string GetBaseContent(this ProjectFile projectFile)
    {
        string @namespace = $"{projectFile.Path.Replace(System.IO.Path.DirectorySeparatorChar.ToString(), ".").Split(":").Last()[1..^(projectFile.Name.Length + 1)]}";
        return $"""
               usingBegin;
               using Fiona.Hosting.Controller.Attributes;
               using Fiona.Hosting.Routing;
               using Fiona.Hosting.Routing.Attributes;
               usingEnd;
               namespace: {@namespace};
               class {projectFile.Name[..^(ProjectFile.Extension.Length + 1)]};
               """;
    }
}