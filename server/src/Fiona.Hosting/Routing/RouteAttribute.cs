namespace Fiona.Hosting.Routing;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public sealed class RouteAttribute: Attribute
{
    public HttpMethodType HttpMethodType { get; }
    public string Route { get; }

    public RouteAttribute(HttpMethodType httpMethodType, string route)
    {
        HttpMethodType = httpMethodType;
        Route = route;
    }

    public RouteAttribute(HttpMethodType httpMethodType)
    {
        HttpMethodType = httpMethodType;
        Route = string.Empty;
    }
}