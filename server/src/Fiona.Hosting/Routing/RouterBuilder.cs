using System.Reflection;
using System.Text;
using Fiona.Hosting.Routing.Attributes;

namespace Fiona.Hosting.Routing;

internal sealed class RouterBuilder
{
    private readonly IList<Type> _controllers = new List<Type>();

    public RouterBuilder AddController(Type controller)
    {
        _controllers.Add(controller);
        return this;
    }

    public Router Build(IServiceProvider provider)
    {
        return new Router(BuildRouteTree(), provider);
    }

    private RouteNode BuildRouteTree()
    {
        Dictionary<string, Dictionary<HttpMethodType, MethodInfo>> routes = GenerateRoutesDictionary();

        var head = GenerateRouteTree(routes);

        return head;
    }

    private static RouteNode GenerateRouteTree(Dictionary<string, Dictionary<HttpMethodType, MethodInfo>> routes)
    {
        RouteNode head = RouteNode.GetHead();
        var sortedKeys = routes.Keys.OrderBy(key => key.Length).ToList();

        foreach (var key in sortedKeys)
        {
            foreach (var (httpMethodType, method) in routes[key])
            {
                head.Insert(httpMethodType, method, key);
            }
        }

        return head;
    }

    private Dictionary<string, Dictionary<HttpMethodType, MethodInfo>> GenerateRoutesDictionary()
    {
        Dictionary<string, Dictionary<HttpMethodType, MethodInfo>> routes = new() { };
        foreach (var controller in _controllers)
        {
            var baseRoute = RouteAttributeUtils.GetBaseRoute(controller);
            InsertRoutesForMethodsInController(controller, baseRoute, routes);
        }

        return routes;
    }

    private static void InsertRoutesForMethodsInController(Type controller, string baseRoute, Dictionary<string, Dictionary<HttpMethodType, MethodInfo>> routes)
    {
        StringBuilder urlBuilder = new(50);
        foreach (var method in controller.GetMethods(BindingFlags.Public | BindingFlags.Instance |
                                                     BindingFlags.DeclaredOnly))
        {
            var (route, methodTypes) = RouteAttributeUtils.GetMetadataFromRouteAttribute(method);
            urlBuilder.Append(baseRoute);
            urlBuilder.Append(route);
            string url = urlBuilder.ToString();
            InsertRoute(routes, url, methodTypes, method);
            urlBuilder.Clear();
        }
    }

    private static void InsertRoute(Dictionary<string, Dictionary<HttpMethodType, MethodInfo>> routes, string url, HttpMethodType methodTypes, MethodInfo method)
    {
        bool routeExists = routes.TryGetValue(url, out Dictionary<HttpMethodType, MethodInfo>? value);

        if (routeExists)
        {
            UpdateRoute(methodTypes, method, value!);
        }
        else
        {
            AddNewRoute(routes, url, methodTypes, method);
        }
    }

    private static void UpdateRoute(HttpMethodType methodTypes, MethodInfo method, Dictionary<HttpMethodType, MethodInfo> value)
    {
        foreach (var methodType in methodTypes.GetMethodTypes())
        {
            if (value.TryGetValue(methodType, out var conflictingMethod))
            {
                throw new RouteConflictException(method.DeclaringType!.FullName!,
                    conflictingMethod.DeclaringType!.FullName!);
            }

            value.Add(methodType, method);
        }
    }
    
    private static void AddNewRoute(Dictionary<string, Dictionary<HttpMethodType, MethodInfo>> routes, string url, HttpMethodType methodTypes, MethodInfo method)
    {
        routes.Add(url, new Dictionary<HttpMethodType, MethodInfo>());
        foreach (var methodType in methodTypes.GetMethodTypes())
        {
            routes[url].Add(methodType, method);
        }
    }


}