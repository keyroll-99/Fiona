using System.Reflection;

namespace Fiona.Hosting.Routing;

internal sealed class RouteNode(string route)
{
    private string _route = route;
    private readonly IList<RouteNode> _children = new List<RouteNode>();
    private readonly IDictionary<HttpMethodType, MethodInfo> _actions = new Dictionary<HttpMethodType, MethodInfo>();

    public void AddAction(HttpMethodType methodType, MethodInfo method)
    {
        _actions.Add(methodType, method);
    }

    public void Insert(HttpMethodType methodType, MethodInfo method, string route)
    {
        if (_route == string.Empty)
        {
            AddAction(methodType, method);
            return;
        }
        Insert(methodType, method, route, 0);
        
    }

    private void Insert(HttpMethodType methodType, MethodInfo method, string route, int depth)
    {
        
    }

    private void AddChild(RouteNode node)
    {
        _children.Add(node);
    }
}