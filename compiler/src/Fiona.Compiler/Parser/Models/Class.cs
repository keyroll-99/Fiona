using System.Text;

namespace Fiona.Compiler.Parser.Models;

internal sealed class Class(IReadOnlyCollection<Endpoint> endpoints, string? route, string name, IReadOnlyCollection<Dependency>? dependencies)
{
    private readonly IReadOnlyCollection<Endpoint> _endpoints = endpoints;

    public string GenerateSourceCode()
    {
        StringBuilder sourceCode = new(1000);
        AddAttribute(sourceCode);
        AddClassDeclaration(sourceCode);
        AddDependencies(sourceCode);
        AddConstructor(sourceCode);
        AddEndpoints(sourceCode);
        sourceCode.Append('}');

        return sourceCode.ToString();
    }

    private void AddAttribute(StringBuilder sourceCode)
    {
        if (route is not null)
        {
            sourceCode.AppendLine($"[Controller(\"{route}\")]");
        }
    }

    private void AddClassDeclaration(StringBuilder sourceCode)
    {
        sourceCode.AppendLine($"public class {name}\n{{");
    }

    private void AddDependencies(StringBuilder sourceCode)
    {
        if (dependencies is null)
        {
            return;
        }
        foreach (Dependency dependency in dependencies)
        {
            sourceCode.AppendLine($"private readonly {dependency.FullDeclaration};");
        }
    }

    private void AddConstructor(StringBuilder sourceCode)
    {
        if (dependencies is null)
        {
            return;
        }
        sourceCode.AppendLine($"public {name}({string.Join(", ", dependencies.Select(x => $"{x.FullDeclaration}"))})");
        sourceCode.AppendLine("{");
        foreach (Dependency dependency in dependencies)
        {
            sourceCode.AppendLine($"this.{dependency.Name} = {dependency.Name};");
        }
        sourceCode.AppendLine("}");
    }

    private void AddEndpoints(StringBuilder sourceCode)
    {
        foreach (Endpoint endpoint in _endpoints)
        {
            sourceCode.AppendLine(endpoint.BuildSourceCode());
        }
    }
}