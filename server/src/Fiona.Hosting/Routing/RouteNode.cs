using System.Reflection;
using System.Text.Json;
using Fiona.Hosting.Controller;
using Fiona.Hosting.Routing.Attributes;

namespace Fiona.Hosting.Routing;

internal sealed class RouteNode
{
    public Dictionary<HttpMethodType, MethodInfo> Actions { get; } = new();
    private readonly Dictionary<HttpMethodType, ParameterInfo> _bodyParameters = new();

    private readonly Dictionary<HttpMethodType, HashSet<string>>
        _queryParameters = new(); // string, Guid, primitive types

    private readonly Url _route;
    private readonly List<RouteNode> _children = [];

    private RouteNode(Url route)
    {
        _route = route;
    }

    public static RouteNode GetHead() => new(string.Empty);

    public void Insert(HttpMethodType methodType, MethodInfo method, Url route)
    {
        bool isHead = route.OriginalUrl == string.Empty;
        if (isHead)
        {
            AddAction(methodType, method);
            return;
        }

        Insert(methodType, method, route, 0);
    }

    public RouteNode? FindNode(Url route)
    {
        if (route.Equals(_route))
        {
            return this;
        }

        RouteNode? next = _children.FirstOrDefault(ch => route.ContainsIn(ch._route));
        return next?.FindNode(route);
    }

    public async Task<object?[]> GetParameters(Uri uri, HttpMethodType methodType, Stream? body)
    {
        List<object> parameters = new();
        object? bodyParameter = await GetBodyParameter(methodType, body);
        if(bodyParameter is not null)
        {
            parameters.Add(bodyParameter);
        }

        return parameters.ToArray();
    }
    
    // TODO: Refactor
    private async Task<object?> GetBodyParameter(HttpMethodType methodType, Stream? body)
    {
        MethodInfo method = Actions[methodType];
        ParameterInfo[] methodParameters = method.GetParameters();
        if (!methodType.HasBody())
        {
            return null;
        }

        if (methodParameters.Length == 1)
        {
            if (body is null)
            {
                return methodParameters[0].DefaultValue;
            }

            return await JsonSerializer.DeserializeAsync(body, methodParameters[0].ParameterType);
        }

        ParameterInfo? bodyParameter =
            _bodyParameters.GetValueOrDefault(methodType);

        if (bodyParameter is not null)
        {
            return body is not null
                ? await JsonSerializer.DeserializeAsync(body, bodyParameter.ParameterType)
                : bodyParameter.DefaultValue;
        }
        
        return null;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_route, Actions);
    }

    private void Insert(HttpMethodType methodType, MethodInfo method, Url route, int depth)
    {
        if (route.SplitUrl.Length == (depth + 1))
        {
            RouteNode? child = _children.FirstOrDefault(ch => ch._route.Equals(route));
            if (child is null)
            {
                child = new RouteNode(route);
                AddChild(child);
            }

            child.AddAction(methodType, method);
            return;
        }

        RouteNode? next = _children.FirstOrDefault(ch => route.ContainsIn(ch._route));
        if (next is null)
        {
            next = new RouteNode(route.GetPartOfUrl(depth + 1));
            AddChild(next);
        }

        next.Insert(methodType, method, route, depth + 1);
    }

    private void AddAction(HttpMethodType methodType, MethodInfo method)
    {
        Actions.Add(methodType, method);

        ParameterInfo[] parameters = method.GetParameters();
        AddBodyParameter(methodType, parameters);
    }

    private void AddBodyParameter(HttpMethodType methodType, ParameterInfo[] parameters)
    {
        if (!methodType.HasBody())
        {
            return;
        }

        ParameterInfo? bodyParameter =
            parameters.FirstOrDefault(p => p.GetCustomAttribute<BodyAttribute>() is not null) ??
            parameters.FirstOrDefault(p => !p.GetCustomAttributes().Any());

        if (bodyParameter is not null)
        {
            _bodyParameters.Add(methodType, bodyParameter);
        }
    }

    private void AddChild(RouteNode node)
    {
        _children.Add(node);
    }
}