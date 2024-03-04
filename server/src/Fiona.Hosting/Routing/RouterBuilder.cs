using System.Reflection;
using System.Text;
using Fiona.Hosting.Controller;
using Fiona.Hosting.Exceptions;

namespace Fiona.Hosting.Routing;

internal sealed class RouterBuilder
{
    private readonly IList<Type> _controllers = new List<Type>();

    public RouterBuilder AddController(Type controller)
    {
        _controllers.Add(controller);
        return this;
    }

    public Router Build()
    {
        return new Router(BuildRouteTree());
    }

    // Todo: refactor this method
    private RouteNode BuildRouteTree()
    {
        RouteNode head = new RouteNode(string.Empty);
        Dictionary<string, Dictionary<HttpMethodType, MethodInfo>> routes = new() { };
        foreach (var controller in _controllers)
        {
            string baseRoute;
            RouteAttribute? controllerRouteAttribute =
                (RouteAttribute?)controller.GetCustomAttribute(typeof(RouteAttribute));
            if (controllerRouteAttribute is not null)
            {
                baseRoute = controllerRouteAttribute.Route;
            }
            else
            {
                ControllerAttribute? controllerAttribute =
                    (ControllerAttribute?)controller.GetCustomAttribute(typeof(ControllerAttribute));
                MissingControllerAttributeException.ThrowIfNull(controllerAttribute);
                baseRoute = controllerAttribute!.Route;
            }

            StringBuilder urlBuilder = new(50);
            foreach (var method in controller.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                urlBuilder.Append(baseRoute);
                RouteAttribute? routeAttribute = (RouteAttribute?)method.GetCustomAttribute(typeof(RouteAttribute));

                if (routeAttribute?.Route is not null)
                {
                    if (!routeAttribute.Route.StartsWith('/'))
                    {
                        urlBuilder.Append('/');
                    }

                    urlBuilder.Append(routeAttribute.Route);
                }

                HttpMethodType methodTypes = routeAttribute?.HttpMethodType ?? HttpMethodType.Get;
                string url = urlBuilder.ToString();
                if (routes.TryGetValue(url, out Dictionary<HttpMethodType, MethodInfo>? value))
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
                else
                {
                    routes.Add(url, new Dictionary<HttpMethodType, MethodInfo>());
                    foreach (var methodType in methodTypes.GetMethodTypes())
                    {
                        routes[url].Add(methodType, method);
                    }
                }

                urlBuilder.Clear();
            }
        }

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
}