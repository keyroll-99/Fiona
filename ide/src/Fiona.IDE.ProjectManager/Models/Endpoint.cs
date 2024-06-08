namespace Fiona.IDE.ProjectManager.Models;

public sealed class Endpoint
{
    public string Name { get; }
    public string? Route { get; }
    public List<string> Methods { get; }
    public string ReturnType { get; }
    public List<Input> Inputs { get; }
    public string Body { get; }

    public Endpoint(string name, string? route, List<string> methods, string returnType, List<Input> inputs, string body)
    {
        Route = route;
        Methods = methods;
        ReturnType = returnType;
        Inputs = inputs;
        Body = body;
        Name = name;
    }
}