using System.Reflection;

namespace Fiona.Hosting.Exceptions;

public class RouteConflictException : Exception
{
    public RouteConflictException(string method1, string method2) : base($"Route conflict between {method1} and {method2}.")
    {
    }
}