using System.Net;
using System.Reflection;
using System.Text.Json;
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

    public async Task<ObjectResult> CallEndpoint(Uri uri, HttpMethodType methodType, Stream? body)
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

        return await InvokeEndpoint(methodInfo, controller, body);
    }

    private RouteNode? GetNode(Uri uri, HttpMethodType methodType)
    {
        return _head.FindNode(uri.AbsolutePath[1..]);
    }

    private static async Task<ObjectResult> InvokeEndpoint(MethodInfo methodInfo, object? controller, Stream? body)
    {
        Type returnType = methodInfo.ReturnType;
        var endpointParameters = methodInfo.GetParameters();

        var parameters = await GetEndpointParameters(body, endpointParameters);

        if (typeof(Task).IsAssignableTo(returnType))
        {
            await ((Task)methodInfo.Invoke(controller, parameters)!);
            return new ObjectResult(null, HttpStatusCode.OK);
        }

        object? result = methodInfo.Invoke(controller, parameters);

        if (result is not null && returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
        {
            dynamic resultTask = result;

            return ObjectResult.FromObject(await resultTask);
        }

        return await Task.FromResult(new ObjectResult(result, HttpStatusCode.OK));
    }

    private static async Task<object[]> GetEndpointParameters(Stream? body, ParameterInfo[] endpointParameters)
    {
        List<object> parameters = [];
        if (endpointParameters.Length > 0)
        {
            if (endpointParameters.Length == 1 || endpointParameters.Any(p =>
                    p.CustomAttributes.Any(a => a.AttributeType == typeof(BodyAttribute))))
            {
                parameters.Add(await JsonSerializer.DeserializeAsync(body, endpointParameters[0].ParameterType));
            }
        }

        return parameters.ToArray();
    }
}