namespace Fiona.Hosting.Routing.Exceptions;

public class ConflictNameOfRouteParametersException(string route)
    : Exception($"At route {route} there are parameters with the same name");