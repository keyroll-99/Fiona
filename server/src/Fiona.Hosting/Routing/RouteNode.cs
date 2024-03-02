using System.Reflection;

namespace Fiona.Hosting.Routing;

internal sealed class RouteNode(string route)
{
    private string _route = route;
    private readonly IList<RouteNode> _children = new List<RouteNode>();
    private readonly IDictionary<string, MethodInfo> _actions = new Dictionary<string, MethodInfo>();

    public void AddAction(string action, MethodInfo method)
    {
        _actions.Add(action, method);
    }
    
    public void AddChild(RouteNode node)
    {
        _children.Add(node);
    }
}