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
        RouteNode? node = _head.FindNode(uri.AbsolutePath[1..]);

        if (node is null)
        {
            return await Task.FromResult(new ObjectResult(null, HttpStatusCode.NotFound));
        }

        MethodInfo? methodInfo = node.GetAction(methodType);
        if (methodInfo is null)
        {
            return await Task.FromResult(new ObjectResult(null, HttpStatusCode.NotFound));
        }

        object? controller = _provider.GetService(methodInfo.DeclaringType!);
        Type returnType = methodInfo.ReturnType;
        
        if (typeof(Task).IsAssignableTo(returnType))
        {
            await ((Task)methodInfo.Invoke(controller, [])!);
            // TODO: Catch exceptions and return 500
            return await Task.FromResult(new ObjectResult(null, HttpStatusCode.OK));
        }

        object? result = methodInfo.Invoke(controller, []);
        if(result is not null && returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
        {
            dynamic resultTask = result;
            await resultTask;
            
            return await Task.FromResult(new ObjectResult(resultTask.Result, HttpStatusCode.OK));
        }
        
        return await Task.FromResult(new ObjectResult(result, HttpStatusCode.OK));
    }
}