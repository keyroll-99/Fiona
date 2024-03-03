namespace Fiona.Hosting.Controller;

[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public sealed class ControllerAttribute : Attribute
{
    public string Route { get; } = string.Empty;

    public ControllerAttribute()
    {
    }

    public ControllerAttribute(string route)
    {
        Route = route;
    }
}