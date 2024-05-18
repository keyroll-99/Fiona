using System.Text;

namespace Fiona.IDE.Compiler.Parser.Models;

internal sealed class Class(IReadOnlyCollection<Endpoint> endpoints, string? route, string name)
{
    private IReadOnlyCollection<Endpoint> _endpoints = endpoints;

    public string GenerateSourceCode()
    {
        StringBuilder sourceCode = new(1000);
        if (route is not null)
        {
            sourceCode.AppendLine($"[Controller(\"{route}\")]");
        }
        
        sourceCode.AppendLine($"public class {name}()\n{{");

        foreach (Endpoint endpoint in _endpoints)
        {
            sourceCode.Append(endpoint.BuildSourceCode());
            sourceCode.Append('\n');
        }
        
        sourceCode.Append('}');

        return sourceCode.ToString();
    }
}