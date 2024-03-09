namespace Fiona.Hosting.Routing;

public class RouteConflictException(string method1, string method2)
    : Exception($"Route conflict between {method1} and {method2}.");