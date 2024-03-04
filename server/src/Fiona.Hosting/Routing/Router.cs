using System.Reflection;

namespace Fiona.Hosting.Routing;

internal sealed class Router
{
    private RouteNode _head;
    
    internal Router(RouteNode head)
    {
        _head = head;
    }
    
}