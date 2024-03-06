using System.Net;
using System.Reflection;
using Fiona.Hosting.Controller;

namespace Fiona.Hosting.Routing;

internal sealed class Router
{
    private readonly RouteNode _head;
    private readonly IServiceProvider _provider;

    internal Router(RouteNode head, IServiceProvider provider)
    {
        _head = head;
        _provider = provider;
    }

    public async Task<ObjectResult> CallEndpoint(Uri uri, HttpMethodType methodType)
    {
        RouteNode? routeNode = GetNode(uri, methodType);

        if (routeNode is null)
        {
            return new ObjectResult(null, HttpStatusCode.NotFound);
        }
        
        MethodInfo? methodInfo = routeNode.GetAction(methodType);

        if (methodInfo is null)
        {
            return new ObjectResult(null, HttpStatusCode.MethodNotAllowed);
        }

        object? controller = _provider.GetService(methodInfo.DeclaringType!);

        return await InvokeEndpoint(methodInfo, controller);
    }

    private RouteNode?GetNode(Uri uri, HttpMethodType methodType)
    {
        return _head.FindNode(uri.AbsolutePath[1..]);
    }

    private static async Task<ObjectResult> InvokeEndpoint(MethodInfo methodInfo, object? controller)
    {
        Type returnType = methodInfo.ReturnType;

        if (typeof(Task).IsAssignableTo(returnType))
        {
            await ((Task)methodInfo.Invoke(controller, [])!);
            return await Task.FromResult(new ObjectResult(null, HttpStatusCode.OK));
        }

        object? result = methodInfo.Invoke(controller, []);

        if (result is not null && returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
        {
            dynamic resultTask = result;

            return ObjectResult.FromObject(await resultTask);
        }

        return await Task.FromResult(new ObjectResult(result, HttpStatusCode.OK));
    }
}