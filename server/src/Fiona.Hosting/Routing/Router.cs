using System.Reflection;

namespace Fiona.Hosting.Routing;

internal sealed class Router
{
    private readonly RouteNode _head;
    
    internal Router(RouteNode head)
    {
        _head = head;
    }

    public Task<object?>? CallEndpoint(Uri uri, HttpMethodType methodType, IServiceProvider provider)
    {
        RouteNode? node = _head.FindNode(uri.AbsolutePath[1..]);

        if (node is null)
        {
            return Task.FromResult<object?>(new {Error = "not found"});
        }
        
        
        MethodInfo? methodInfo = node.CallAction(methodType);
        if (methodInfo is null)
        {
            return Task.FromResult<object?>(new {Error = "not found"});
        }
        
        var controller = provider.GetService(methodInfo.DeclaringType);
        return (Task<object?>)methodInfo.Invoke(controller, []);
    }
    
}