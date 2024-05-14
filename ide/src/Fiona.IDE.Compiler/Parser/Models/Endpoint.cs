namespace Fiona.IDE.Compiler.Parser.Models;

internal sealed class Endpoint(string name, string? route, string? methodTypes, string body)
{
    private readonly string _name = name;
    private readonly string _route = route;
    private readonly IReadOnlyCollection<HttpMethodType> _methodTypes = GetMethodTypes(methodTypes);
    private readonly string _body = body;


    private static IReadOnlyCollection<HttpMethodType> GetMethodTypes(string methodTypes)
    {
        return [];
    }
}