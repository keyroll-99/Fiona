namespace Fiona.Hosting.Routing;

[Flags]
public enum HttpMethodType
{
    Get = 1 << 0,
    Post = 1 << 1,
    Put = 1 << 2,
    Patch = 1 << 3,
    Delete = 1 << 4
}