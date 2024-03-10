namespace Fiona.Hosting.Routing;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public sealed class RouteAttribute(HttpMethodType httpMethodType, string route, string[] queryParameters) : Attribute
{
    public HttpMethodType HttpMethodType { get; } = httpMethodType;
    public string Route { get; } = route;
    public List<string> QueryParameters { get; } = queryParameters.ToList();

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
}