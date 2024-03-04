namespace Fiona.Hosting.Routing;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public sealed class RouteAttribute(HttpMethodType httpMethodType, string route) : Attribute
{
    public HttpMethodType HttpMethodType { get; } = httpMethodType;
    public string Route { get; } = route;

    public RouteAttribute(HttpMethodType httpMethodType) : this(httpMethodType, string.Empty)
    {
    }
}