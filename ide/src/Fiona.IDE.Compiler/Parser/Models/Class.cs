namespace Fiona.IDE.Compiler.Parser.Models;

internal sealed class Class(IReadOnlyCollection<Endpoint> endpoints, string route)
{
    private IReadOnlyCollection<Endpoint> _endpoints = endpoints;
    private readonly string _route = route;

    public string GenerateSourceCode()
    {
        return "source code";
    }
}