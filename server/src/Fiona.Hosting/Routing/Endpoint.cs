using System.Collections.Specialized;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Web;
using Fiona.Hosting.Controller;
using Fiona.Hosting.Cookie;
using Fiona.Hosting.Routing.Attributes;

namespace Fiona.Hosting.Routing;

internal sealed class Endpoint
{
    private readonly MethodInfo _method;
    private readonly HashSet<string> _routeParameterNames; // string, Guid, primitive types
    private readonly HashSet<string> _queryParameterNames; // string, Guid, primitive types

    private readonly HashSet<(string parameterName, string cookieName)>
        _cookieParameters; // string, Guid, primitive types

    private readonly ParameterInfo? _bodyParameter;
    private readonly RouteAttribute? _routeAttribute;
    private readonly Url _url;

    public Endpoint(MethodInfo method, Url url)
    {
        _method = method;
        _routeAttribute = method.GetCustomAttribute<RouteAttribute>();
        _routeParameterNames = url.GetNameOfUrlParameters();
        _bodyParameter = GetBodyParameter();
        _cookieParameters = GetCookieParameterNames();
        _queryParameterNames = _routeAttribute?.QueryParameters ?? [];
        _url = url;
    }

    public async Task<ObjectResult> Invoke(Uri url, Stream? body, CookieCollection cookies,
        IServiceProvider serviceProvider)
    {
        object? controller = serviceProvider.GetService(_method.DeclaringType!);
        object?[] parameters = await GetParameters(url, body, cookies);
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
            // This will be replace by middleware
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
                                                  _routeAttribute.QueryParameters.Count == 0) &&
                                                 _routeParameterNames.Count == 0;

        if (shouldMatchArgumentAsBodyArgument)
        {
            bodyArgument = parameters[0];
        }

        return bodyArgument;
    }

    private HashSet<(string parameterName, string cookieName)> GetCookieParameterNames()
    {
        var cookieParameter = _method.GetParameters()
            .Where(x => x.GetCustomAttribute<CookieAttribute>() is not null && x.Name is not null)
            .Select(x => (parameterName: x.Name!,
                cookieName: x.GetCustomAttribute<CookieAttribute>()!.Name ?? x.Name!));

        return cookieParameter.ToHashSet();
    }

    private async Task<object?[]> GetParameters(Uri uri, Stream? body, CookieCollection cookies)
    {
        object? bodyParameter = await GetBodyParameter(body);
        IReadOnlyCollection<(object? value, string name)> routeParameters = GetRouteParameters(uri);
        IReadOnlyCollection<(object? value, string name)> queryParameters = GetQueryParameters(uri);
        IReadOnlyCollection<(object? value, string name)> cookieParameters = GetCookies(cookies);
        return CreateParameterArray(bodyParameter, routeParameters, queryParameters, cookieParameters);
    }

    private object?[] CreateParameterArray(object? bodyParameter,
        IReadOnlyCollection<(object? value, string name)> routeParameters,
        IReadOnlyCollection<(object? value, string name)> queryParameters,
        IReadOnlyCollection<(object? value, string name)> cookies)
    {
        List<object?> parameters = new List<object?>(queryParameters.Count + routeParameters.Count + 1);
        foreach (ParameterInfo parameterInfo in _method.GetParameters())
        {
            if(cookies.Any(c => c.name == parameterInfo.Name))
            {
                var cookieValue = cookies.First(c => c.name == parameterInfo.Name).value;
                parameters.Add(Convert.ChangeType(cookieValue, parameterInfo.ParameterType));
                continue;
            }
            
            if (parameterInfo.ParameterType == _bodyParameter?.ParameterType)
            {
                parameters.Add(bodyParameter);
                continue;
            }

            if (routeParameters.Any(p => p.name == parameterInfo.Name))
            {
                var routeParameterValue = routeParameters.First(p => p.name == parameterInfo.Name).value;
                parameters.Add(Convert.ChangeType(routeParameterValue, parameterInfo.ParameterType));
                continue;
            }

            if (queryParameters.Any(p => p.name == parameterInfo.Name))
            {
                var queryParameterValue = queryParameters.First(p => p.name == parameterInfo.Name).value;
                parameters.Add(Convert.ChangeType(queryParameterValue, parameterInfo.ParameterType));
            }
        }

        return parameters.ToArray();
    }

    private IReadOnlyCollection<(object? value, string name)> GetCookies(CookieCollection cookies)
    {
        HashSet<(object? value, string name)> result = [];
        foreach (var (parameterName, cookieName) in _cookieParameters)
        {
            var cookie = cookies.FirstOrDefault(x => x.Name == cookieName);
            result.Add((value: cookie?.Value ?? null, name: parameterName));
        }

        return result;
    }

    private IReadOnlyCollection<(object? value, string name)> GetRouteParameters(Uri uri)
    {
        List<string> routeParameterValues = uri.AbsolutePath[1..].Split("/").ToList();
        List<(object? value, string name)> result = [];
        foreach (var indexesOfParameter in _url.IndexesOfParameters)
        {
            result.Add(
                (value: routeParameterValues[indexesOfParameter], name: _url.SplitUrl[indexesOfParameter][1..^1]));
        }


        return result;
    }

    private IReadOnlyCollection<(object? value, string name)> GetQueryParameters(Uri uri)
    {
        HashSet<(object? value, string name)> result = [];
        NameValueCollection queries = HttpUtility.ParseQueryString(uri.Query);
        foreach (var queryParameter in _queryParameterNames)
        {
            result.Add((value: queries.Get(queryParameter) ?? string.Empty, name: queryParameter));
        }

        return result;
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