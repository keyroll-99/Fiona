using System.Reflection;
using System.Text.Json;
using Fiona.Hosting.Controller;

namespace Fiona.Hosting.Routing;

internal sealed class RouteNode
{
    public Dictionary<HttpMethodType, MethodInfo> Actions { get; } = new();
    private readonly Dictionary<HttpMethodType, ParameterInfo> _bodyParameters = new();
    private readonly string _route;
    private readonly HashSet<string> _queryParameters = []; // string, Guid, primitive types
    private readonly HashSet<string> _routeParameters = []; // string, Guid, primitive types
    private readonly bool _isParameterized;
    private readonly List<RouteNode> _children = [];

    private const string OpenParameter = "{";
    private const string CloseParameter = "}";
    
    public RouteNode(Url route)
    {
        BuildRouteParameters(route.OriginalUrl);
        _isParameterized = route.OriginalUrl.EndsWith(CloseParameter);
        _route = route.NormalizeUrl;
    }
    
    public void Insert(HttpMethodType methodType, MethodInfo method, Url route)
    {
        if (route.OriginalUrl == string.Empty)
        {
            AddAction(methodType, method);
            return;
        }

        Insert(methodType, method, route, 0);
    }

    public RouteNode? FindNode(string route)
    {
        if (route == _route)
        {
            return this;
        }

        RouteNode? next = _children.FirstOrDefault(ch => route.StartsWith(ch._route));
        next ??= _children.FirstOrDefault(ch => ch._isParameterized);
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

    private void Insert(HttpMethodType methodType, MethodInfo method, Url route, int depth)
    {
        if (route.SplitUrl.Length == (depth + 1))
        {
            RouteNode? child = _children.FirstOrDefault(ch => ch._route == route.NormalizeUrl);
            if (child is null)
            {
                child = new RouteNode(route);
                AddChild(child);
            }

            child.AddAction(methodType, method);
            return;
        }

        RouteNode? next = _children.FirstOrDefault(ch => route.NormalizeUrl.StartsWith(ch._route));
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
    
    private void BuildRouteParameters(string route)
    {
        if (!route.Contains(OpenParameter))
        {
            return;
        }

        int offset = 0;
        while (true)
        {
            int indexOfOpen = route.IndexOf(OpenParameter, offset, StringComparison.Ordinal);
            if (indexOfOpen == -1)
            {
                break;
            }
            int indexOfClose = route.IndexOf(CloseParameter, offset, StringComparison.Ordinal);
            string variableName = route.Substring(indexOfOpen + 1, indexOfClose - indexOfOpen - 1);
            if (!_routeParameters.Add(variableName))
            {
                throw new ConflictNameOfRouteParameters(route);
            }
            offset = indexOfClose + 1;
        }
    }
}