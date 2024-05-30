using Fiona.IDE.ProjectManager.Exceptions;
using System.Text.Json.Serialization;

namespace Fiona.IDE.ProjectManager.Models;

public sealed class ProjectFile
{
    public string Path { get; }
    public string Name { get; }
    public string Namespace { get; }
    
    public const string Extension = "fn";

    private Class @class;


    [JsonConstructor]
    private ProjectFile(string path)
    {
        Path = path;
        Name = path.Split(System.IO.Path.DirectorySeparatorChar).Last();
        Namespace = path.Replace(System.IO.Path.DirectorySeparatorChar.ToString(), ".").Split(":").Last().Replace(".fn", "")[1..];
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
        
        ProjectFile projectFile = new(path);

        await projectFile.SaveContentAsync(projectFile.GetBaseContent());
        
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
               namespace: {projectFile.Namespace}
               class: {projectFile.Name}
               """;
    }

}