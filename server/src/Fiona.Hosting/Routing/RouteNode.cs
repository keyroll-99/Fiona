using System.Reflection;
using System.Text.Json;
using Fiona.Hosting.Controller;

namespace Fiona.Hosting.Routing;

internal sealed class RouteNode
{
    public Dictionary<HttpMethodType, MethodInfo> Actions { get; } = new();
    private readonly Dictionary<HttpMethodType, ParameterInfo> _bodyParameters = new();
    private readonly Url _route;
    private readonly HashSet<string> _queryParameters = []; // string, Guid, primitive types
    private readonly List<RouteNode> _children = [];
    
    private RouteNode(Url route)
    {
        _route = route;
    }
    
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

        RouteNode? next = _children.FirstOrDefault(ch => route.NormalizeUrl.StartsWith(ch._route.NormalizeUrl));
        return next?.FindNode(route);
    }

    public async Task<object?[]> GetEndpointParameters(Uri uri, HttpMethodType methodType, Stream? body)
    {
        List<object?> parameters = [];
        MethodInfo method = Actions[methodType];
        ParameterInfo[] methodParameters = method.GetParameters();
        if (methodParameters.Length == 1 && methodType.HasBody())
        {
            if (body is null)
            {
                return [methodParameters[0].DefaultValue];
            }

            return [await JsonSerializer.DeserializeAsync(body, methodParameters[0].ParameterType)];
        }

        if (methodType.HasBody())
        {
            ParameterInfo? bodyParameter =
                _bodyParameters.GetValueOrDefault(methodType);

            if (bodyParameter is not null)
            {
                parameters.Add(body is not null
                    ? await JsonSerializer.DeserializeAsync(body, bodyParameter.ParameterType)
                    : bodyParameter.DefaultValue);
            }
        }

        return parameters.ToArray();
    }

    public static RouteNode GetHead() => new(string.Empty);

    private void Insert(HttpMethodType methodType, MethodInfo method, Url route, int depth)
    {
        if (route.SplitUrl.Length == (depth + 1))
        {
            RouteNode? child = _children.FirstOrDefault(ch => ch._route.Equals(route.NormalizeUrl));
            if (child is null)
            {
                child = new RouteNode(route);
                AddChild(child);
            }

            child.AddAction(methodType, method);
            return;
        }

        RouteNode? next = _children.FirstOrDefault(ch => route.NormalizeUrl.StartsWith(ch._route.NormalizeUrl));
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