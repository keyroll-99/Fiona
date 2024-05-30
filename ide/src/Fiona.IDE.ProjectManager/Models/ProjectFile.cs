using Fiona.IDE.ProjectManager.Exceptions;
using System.Text.Json.Serialization;

namespace Fiona.IDE.ProjectManager.Models;

public sealed class ProjectFile
{
    public string Path { get; }
    public string Name { get; }
    public Class Class { get; }
    public const string Extension = "fn";


    [JsonConstructor]
    private ProjectFile(string path)
    {
        Path = path;
        Name = path.Split(System.IO.Path.DirectorySeparatorChar).Last();
        Class = Class.Load(path).GetAwaiter().GetResult();
    }

    private ProjectFile(string path, Class @class)
    {
        Path = path;
        Name = path.Split(System.IO.Path.DirectorySeparatorChar).Last();
        Class = @class;
    }

    internal static async Task<ProjectFile> Create(string path)
    {
        if (File.Exists(path))
        {
            throw new FileAlreadyExistsException(path);
        }
        FileStream fileHandler = File.Create(path);
        // We have to close file after create
        fileHandler.Close();
        
        ProjectFile projectFile = new(path, null!); // dommy class create to create inital conent

        await projectFile.SaveContentAsync(projectFile.GetBaseContent());

        projectFile = new ProjectFile(path);
        
        return projectFile;
    }
    
    private async Task SaveContentAsync(string content)
    {
        await File.WriteAllTextAsync(Path, content);
    }
        
}

internal static class ProjectFileExtensions
{
    public static string GetBaseContent(this ProjectFile projectFile)
    {
        return $"""
               usingBegin;
               using Fiona.Hosting.Controller.Attributes;
               using Fiona.Hosting.Routing;
               using Fiona.Hosting.Routing.Attributes;
               usingEnd;
               namespace: {projectFile.Path.Replace(System.IO.Path.DirectorySeparatorChar.ToString(), ".").Split(":").Last().Replace(".fn", "")[1..]};
               class {projectFile.Name};
               """;
    }

}