using System.Net;
using Fiona.Hosting.Controller;

namespace Fiona.Hosting.Routing;


internal sealed class Router
{
    private readonly RouteNode _head;
    private readonly IServiceProvider _serviceProvider;

    internal Router(RouteNode head, IServiceProvider serviceProvider)
    {
        _head = head;
        _serviceProvider = serviceProvider;
    }

    public Task<ObjectResult> CallEndpoint(Uri uri, HttpMethodType methodType, Stream? body)
    {
        RouteNode? routeNode = GetNode(uri);

        if (routeNode is null)
        {
            return Task.FromResult(new ObjectResult(null, HttpStatusCode.NotFound));
        }
        
        Endpoint? endpoint = routeNode.Actions.GetValueOrDefault(methodType);
        return endpoint is null ? Task.FromResult(new ObjectResult(null, HttpStatusCode.MethodNotAllowed)) : endpoint.Invoke(uri, body, _serviceProvider);
    }

    private RouteNode? GetNode(Uri uri)
    {
        return _head.FindNode(uri.AbsolutePath[1..]);
    }

}