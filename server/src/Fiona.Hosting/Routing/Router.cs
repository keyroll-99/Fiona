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

    public async Task<ObjectResult<object>> CallEndpoint(Uri uri, HttpMethodType methodType)
    {
        RouteNode? node = _head.FindNode(uri.AbsolutePath[1..]);

        if (node is null)
        {
            return await Task.FromResult(new ObjectResult<object>(null, HttpStatusCode.NotFound));
        }

        MethodInfo? methodInfo = node.GetAction(methodType);
        if (methodInfo is null)
        {
            return await Task.FromResult(new ObjectResult<object>(null, HttpStatusCode.NotFound));
        }

        var controller = _provider.GetService(methodInfo.DeclaringType!);
        var resultTask = (Task)methodInfo.Invoke(controller, [])!;

        await resultTask.ConfigureAwait(false);

        object result = (object)((dynamic)resultTask).Result;
        
        if (result is IResult)
        {
            return result as ObjectResult<object>;
        }

        return new ObjectResult<object>(result, HttpStatusCode.OK);
    }
}