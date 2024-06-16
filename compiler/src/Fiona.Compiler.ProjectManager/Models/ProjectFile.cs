using Fiona.Compiler.ProjectManager.Exceptions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fiona.Compiler.ProjectManager.Models;

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

    public async Task Save()
    {
        StringBuilder fileContentBuilder = new();
        AddUsingContent(fileContentBuilder);
        AddNamespaceContent(fileContentBuilder);
        AddClassContent(fileContentBuilder);

        await using FileStream file = new(Path, FileMode.Create);
        file.Close();
        await SaveContentAsync(fileContentBuilder.ToString());
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

        await using (File.Create(path))
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

    private void AddUsingContent(StringBuilder fileContentBuilder)
    {
        fileContentBuilder.AppendLine("usingBegin;");
        foreach (string @using in Class!.Usings)
        {
            fileContentBuilder.AppendLine($"using {@using};");
        }
        fileContentBuilder.AppendLine("usingEnd;");
    }

    private void AddNamespaceContent(StringBuilder fileContentBuilder)
    {
        fileContentBuilder.AppendLine($"namespace: {Class!.Namespace};");
    }

    private void AddClassContent(StringBuilder fileContentBuilder)
    {
        fileContentBuilder.AppendLine($"{Class!.ToString()};");
        if (!string.IsNullOrWhiteSpace(Class.Route))
        {
            fileContentBuilder.AppendLine($"route: {Class.Route};");
        }

        if (Class.Dependencies.Count > 0)
        {
            fileContentBuilder.AppendLine($"inject: ");
            foreach (Dependency dependency in Class.Dependencies)
            {
                fileContentBuilder.AppendLine($"- {dependency.ToString()}");
            }
            fileContentBuilder.AppendLine(";");
        }

        AddMethodContent(fileContentBuilder);
    }

    private void AddMethodContent(StringBuilder fileContentBuilder)
    {
        foreach (Endpoint endpoint in Class.Endpoints)
        {
            fileContentBuilder.AppendLine($"endpoint: {endpoint.Name}");
            if (endpoint.Route is not null)
            {
                fileContentBuilder.AppendLine($"route: {endpoint.Route};");
            }
            if (endpoint.Methods?.Count > 0)
            {
                fileContentBuilder.AppendLine($"method: [{string.Join(',', endpoint.Methods)}];");
            }
            fileContentBuilder.AppendLine($"return: {endpoint.ReturnType};");
            if (endpoint.Inputs.Count > 0)
            {
                fileContentBuilder.AppendLine("input: ");
                foreach (Input input in endpoint.Inputs)
                {
                    fileContentBuilder.AppendLine($"- {input.ToString()}");
                }
                fileContentBuilder.AppendLine(";");
            }

            fileContentBuilder.AppendLine("bodyBegin;");
            fileContentBuilder.Append(endpoint.Body);
            fileContentBuilder.AppendLine("bodyEnd;");
        }
    }
    public override int GetHashCode() => HashCode.Combine(Path);
}

internal static class ProjectFileExtensions
{
    public static string GetBaseContent(this ProjectFile projectFile)
    {
        string @namespace = $"{projectFile.Path.Replace(Path.DirectorySeparatorChar.ToString(), ".").Split(":").Last()[1..^(projectFile.Name.Length + 1)]}";
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