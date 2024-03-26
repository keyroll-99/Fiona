namespace Fiona.Hosting.Controller.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ControllerAttribute : Attribute
{
    public ControllerAttribute()
    {
    }

    public ControllerAttribute(string route)
    {
        Route = route;
    }

    public string Route { get; } = string.Empty;
}