namespace Fiona.IDE.ProjectManager.Models;

public sealed class Dependency(string name, string type)
{
    public string Name { get; } = name;
    public string Type { get; } = type;

}