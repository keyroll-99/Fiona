using System.Net;
using System.Reflection;
using System.Text.Json;
using Fiona.Hosting.Controller;
using Fiona.Hosting.Routing.Attributes;

namespace Fiona.Hosting.Routing;

internal sealed class Endpoint
{
    private readonly MethodInfo _method;
    private HashSet<string> _routeParameterNames; // string, Guid, primitive types
    private readonly HashSet<string> _queryParameterNames; // string, Guid, primitive types
    private readonly ParameterInfo? _bodyParameter;
    private readonly RouteAttribute? _routeAttribute;
    private readonly Url _url;

    public Endpoint(MethodInfo method, Url url)
    {
        _method = method;
        _routeAttribute = method.GetCustomAttribute<RouteAttribute>();
        _routeParameterNames = url.GetUrlParameters();
        _bodyParameter = GetBodyParameter();
        _queryParameterNames = GetQueryParameters();
        _url = url;
    }

    public async Task<ObjectResult> Invoke(Uri url, Stream? body, IServiceProvider serviceProvider)
    {
        object? controller = serviceProvider.GetService(_method.DeclaringType!);
        object?[] parameters = await GetParameters(url, body);
        Type returnType = _method.ReturnType;

        return await InvokeAndCastResultToObjectResult(returnType, controller, parameters);
    }

    private async Task<ObjectResult> InvokeAndCastResultToObjectResult(Type returnType, object? controller,
        object?[] parameters)
    {
        if (typeof(Task).IsAssignableTo(returnType))
        {
            await (Task)_method.Invoke(controller, parameters)!;
            return new ObjectResult(null, HttpStatusCode.OK);
        }

        if (typeof(void).IsAssignableTo(returnType))
        {
            _method.Invoke(controller, parameters);
            return new ObjectResult(null, HttpStatusCode.OK);
        }

        try
        {
            object? result = _method.Invoke(controller, parameters);

            return await CastResultToObjectResult(result, returnType);
        }
        catch (Exception e)
        {
            return new ObjectResult(e.ToString(), HttpStatusCode.InternalServerError);
        }
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
        List<object> parameters = [];
        object? bodyParameter = await GetBodyParameter(body);
        IEnumerable<object?> routeParameters = GetRouteParameters(uri);
        IEnumerable<object?> queryParameters = GetQueryParameters();

        return parameters.ToArray();
    }

    private IEnumerable<object?> GetRouteParameters(Uri uri)
    {
        List<string> routeParameterValues = uri.AbsolutePath.Split("/").ToList();
        List<object?> result = [];
        result.AddRange(_url.IndexesOfParameters.Select(index => routeParameterValues[index]));

        return result;
    }

    private HashSet<string> GetQueryParameters()
    {
        return _routeAttribute is not null ? _routeAttribute.QueryParameters : [];
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