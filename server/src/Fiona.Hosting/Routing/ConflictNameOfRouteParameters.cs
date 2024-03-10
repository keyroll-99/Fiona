namespace Fiona.Hosting.Routing;

public class ConflictNameOfRouteParameters(string route)
    : Exception($"At route {route} there are parameters with the same name");