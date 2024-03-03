using System.Reflection;

namespace Fiona.Hosting.Routing;

internal sealed class Router
{
    private RouteNode Head;
    
    internal Router(RouteNode head)
    {
        Head = head;
    }
    
}