namespace Fiona.Hosting.Controller;

[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public sealed class ControllerAttribute : Attribute
{
    private string _route = string.Empty;
    
    public ControllerAttribute()
    {
        
    }
    
    public ControllerAttribute(string route)
    {
        _route = route;
    }
}