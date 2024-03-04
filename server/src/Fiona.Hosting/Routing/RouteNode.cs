using System.Reflection;

namespace Fiona.Hosting.Routing;

internal sealed class RouteNode(string route)
{
    private readonly string _route = route;
    private readonly List<RouteNode> _children = [];
    private readonly IDictionary<HttpMethodType, MethodInfo> _actions = new Dictionary<HttpMethodType, MethodInfo>();


    public void Insert(HttpMethodType methodType, MethodInfo method, string route)
    {
        if (route == string.Empty)
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
        return next?.FindNode(route);
    }
    
    public MethodInfo? CallAction(HttpMethodType methodType)
    {
        return _actions.TryGetValue(methodType, out MethodInfo? method) ? method : null;
    }

    private void Insert(HttpMethodType methodType, MethodInfo method, string route, int depth)
    {
        string[] splitRoute = route.Split('/');
        if (splitRoute.Length == (depth + 1))
        {
            RouteNode? child = _children.FirstOrDefault(ch => ch._route == route);
            if (child is null)
            {
                child = new RouteNode(route);
                AddChild(child);
            }

            child.AddAction(methodType, method);
            return;
        }

        RouteNode? next = _children.FirstOrDefault(ch => route.StartsWith(ch._route));
        if (next is null)
        {
            next = new RouteNode(string.Join("/", splitRoute[..(depth + 1)]));            
            AddChild(next);
        }
            
        next.Insert(methodType, method, route, depth + 1);
    }

    private void AddAction(HttpMethodType methodType, MethodInfo method)
    {
        _actions.Add(methodType, method);
    }

    private void AddChild(RouteNode node)
    {
        _children.Add(node);
    }
}