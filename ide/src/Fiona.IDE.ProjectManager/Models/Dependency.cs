namespace Fiona.IDE.ProjectManager.Models;

internal sealed class Dependency(string name, string type)
{
    public string Name { get; } = name;
    public string Type { get; } = type;

}