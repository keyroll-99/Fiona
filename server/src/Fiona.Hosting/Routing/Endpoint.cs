using System.Net;
using System.Reflection;
using System.Text.Json;
using Fiona.Hosting.Controller;
using Fiona.Hosting.Routing.Attributes;

namespace Fiona.Hosting.Routing;

internal sealed class Endpoint
{
    private MethodInfo _method;
    private HashSet<string> _parametersName; // string, Guid, primitive types
    private ParameterInfo? _bodyParameter;
    private HashSet<Stream> _queryParameters;
    private RouteAttribute? _routeAttribute;

    public Endpoint(MethodInfo method, Url url)
    {
        _method = method;
        _routeAttribute = method.GetCustomAttribute<RouteAttribute>();
        _parametersName = url.GetParameters();
        _bodyParameter = GetBodyParameter();
        _queryParameters = GetQueryParameters();
    }

    public async Task<ObjectResult> Invoke(Uri url, Stream? body, IServiceProvider _serviceProvider)
    {
        object? controller = _serviceProvider.GetService(_method.DeclaringType!);
        object?[] parameters = await GetParameters(url, body);
        Type returnType = _method.ReturnType;

        if (typeof(Task).IsAssignableTo(returnType))
        {
            await (Task)_method.Invoke(controller, parameters)!;
            return new ObjectResult(null, HttpStatusCode.OK);
        }

        object? result = _method.Invoke(controller, parameters);
        return await CastResultToObjectResult(result, returnType);
    }

    private static async Task<ObjectResult> CastResultToObjectResult(object? result, Type returnType)
    {
        if (result is not null && returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
        {
            dynamic resultTask = result;
            if (returnType.GetGenericArguments().First().IsAssignableTo(typeof(IResult)))
            {
                return await resultTask;
            }

            return new ObjectResult(await resultTask, HttpStatusCode.OK);
        }

        if (returnType.IsAssignableTo(typeof(IResult)))
        {
            return (ObjectResult)result!;
        }

        return new ObjectResult(result, HttpStatusCode.OK);
    }

    private ParameterInfo? GetBodyParameter()
    {
        ParameterInfo[] parameters = _method.GetParameters();

        ParameterInfo? bodyArgument = parameters
            .FirstOrDefault(p => p.GetCustomAttribute<BodyAttribute>() is not null);

        bool shouldMatchArgumentAsBodyArgument = bodyArgument is null && parameters.Length == 1 &&
                                                 (_routeAttribute is null ||
                                                  _routeAttribute.QueryParameters.Count == 0);

        if (shouldMatchArgumentAsBodyArgument)
        {
            bodyArgument = parameters[0];
        }

        return bodyArgument;
    }

    private async Task<object?[]> GetParameters(Uri uri, Stream? body)
    {
        List<object> parameters = new();
        object? bodyParameter = await GetBodyParameter(body);
        if (bodyParameter is not null)
        {
            parameters.Add(bodyParameter);
        }

        return parameters.ToArray();
    }

    private HashSet<Stream> GetQueryParameters()
    {
        return new();
    }

    private async Task<object?> GetBodyParameter(Stream? body)
    {
        if (_bodyParameter is null)
        {
            return null;
        }

        return body is not null
            ? await JsonSerializer.DeserializeAsync(body, _bodyParameter.ParameterType)
            : _bodyParameter.DefaultValue;
    }
}