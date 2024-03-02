namespace Fiona.Hosting.Routing;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public sealed class RouteAttribute(HttpMethodType httpMethodType, string path)
{
    public HttpMethodType HttpMethodType { get; } = httpMethodType;
    public string Path { get; } = path;
}