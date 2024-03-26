namespace Fiona.Hosting.Routing.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class RouteAttribute(HttpMethodType httpMethodType, string route, string[] queryParameters) : Attribute
{
    public RouteAttribute(HttpMethodType httpMethodType) : this(httpMethodType, string.Empty, [])
    {
    }

    public RouteAttribute(HttpMethodType httpMethodType, string route) : this(httpMethodType, route, [])
    {
    }

    public RouteAttribute(HttpMethodType httpMethodType, string[] queryParameters) : this(httpMethodType, string.Empty,
        queryParameters)
    {
    }

    public HttpMethodType HttpMethodType { get; } = httpMethodType;
    public string Route { get; } = route;
    public HashSet<string> QueryParameters { get; } = queryParameters.ToHashSet();
}