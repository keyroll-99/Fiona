using System.Text;

namespace Fiona.IDE.Compiler.Parser.Models;

internal sealed class Class(IReadOnlyCollection<Endpoint> endpoints, string? route)
{
    private IReadOnlyCollection<Endpoint> _endpoints = endpoints;
    private readonly string? _route = route;

    public string GenerateSourceCode()
    {
        StringBuilder sourceCode = new(1000);
        if (_route is not null)
        {
            sourceCode.AppendLine($"[Controller(\"{_route}\")]");
        }
        
        sourceCode.AppendLine($"public class TestController()\n{{");

        foreach (Endpoint endpoint in _endpoints)
        {
            sourceCode.Append(endpoint.BuildSourceCode());
            sourceCode.Append('\n');
        }
        
        sourceCode.Append('}');

        return sourceCode.ToString();
    }
}