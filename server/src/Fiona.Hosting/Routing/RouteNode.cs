using System.Reflection;

namespace Fiona.Hosting.Routing;


// TODO: Rebuild RouteNode
// Osobne wyszukiwanie routa
// osobne dodawanie
internal sealed class RouteNode
{
    public Dictionary<HttpMethodType, Endpoint> Actions { get; } = new();
    private readonly Url _route;
    private readonly List<RouteNode> _children = [];
    private readonly bool _isParameterized;

    private RouteNode(Url route)
    {
        _route = route;
        _isParameterized = route.NormalizeUrl.EndsWith("}");
    }

    public static RouteNode GetHead() => new(string.Empty);

    public void Insert(HttpMethodType methodType, MethodInfo method, Url url)
    {
        bool isHead = url.NormalizeUrl == string.Empty;
        if (isHead)
        {
            AddAction(methodType, method, url);
            return;
        }

        Insert(methodType, method, url, 0);
    }

    public RouteNode? FindNode(Uri uri)
    {
        if (_route.Equals(uri))
        {
            return this;
        }

        RouteNode? next = _children.FirstOrDefault(ch => ch._route.IsSubUrl(uri) && !ch._isParameterized);
        next ??= _children.FirstOrDefault(ch => ch._route.IsSubUrl(uri) && ch._isParameterized);
        return next?.FindNode(uri);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_route, Actions);
    }

    private void Insert(HttpMethodType methodType, MethodInfo method, Url url, int depth)
    {
        if (url.SplitUrl.Length == (depth + 1))
        {
            RouteNode? child = _children.FirstOrDefault(ch => ch._route.NormalizeUrl == url.NormalizeUrl);
            if (child is null)
            {
                child = new RouteNode(url);
                AddChild(child);
            }

            child.AddAction(methodType, method, url);
            return;
        }

        RouteNode? next = _children.FirstOrDefault(ch => url.NormalizeUrl.StartsWith(ch._route.NormalizeUrl + "/"));
        if (next is null)
        {
            next = new RouteNode(url.GetPartOfUrl(depth + 1));
            AddChild(next);
        }

        next.Insert(methodType, method, url, depth + 1);
    }

    private void AddAction(HttpMethodType methodType, MethodInfo method, Url url)
    {
        Actions.Add(methodType, new Endpoint(method, url));
    }

    private void AddChild(RouteNode node)
    {
        _children.Add(node);
    }
}