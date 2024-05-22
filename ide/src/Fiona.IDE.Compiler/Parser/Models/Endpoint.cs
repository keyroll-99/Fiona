using Fiona.IDE.Compiler.Tokens;
using System.Text;

namespace Fiona.IDE.Compiler.Parser.Models;

internal sealed class Endpoint(string name, string? route, string? methodTypes, string? returnType, IReadOnlyCollection<Parameter> parameters, IReadOnlyCollection<IToken> bodyTokens)
{
    private readonly IReadOnlyCollection<HttpMethodType> _methodTypes = GetMethodTypes(methodTypes);
    private readonly IReadOnlyCollection<IToken> _bodyTokens = bodyTokens;

    public string BuildSourceCode()
    {
        StringBuilder sourceCode = new(1000);

        AppendAttributes(sourceCode);
        AppendMethodDeclaration(sourceCode);
        AppendBody(sourceCode);
        sourceCode.Append('}');

        return sourceCode.ToString();
    }

    private void AppendAttributes(StringBuilder sourceCode)
    {
        string methodTypes = string.Join(" | ", _methodTypes.Select(x => x.GetMethodTypesUsing()));
        string routeValue = route is not null ? $", \"{route}\"" : "";
        IEnumerable<Parameter> queryParameters = parameters.Where(x => x.Type == ParameterType.Query).ToList();
        string parameters1 = queryParameters.Any() ? $", [{string.Join(", ", queryParameters.Select(x => $"\"{x.Name}\""))}]" : string.Empty;

        sourceCode.Append($"[Route({methodTypes}{routeValue}{parameters1})]\n");
    }
    
    private void AppendMethodDeclaration(StringBuilder sourceCode)
    {
        sourceCode.Append("public async Task");
        if (returnType is not null)
        {
            sourceCode.Append($"<{returnType}>");
        }

        string parameters1 = string.Join(", ", parameters.Select(x => x.GenerateSourceCode()));

        sourceCode.Append($" {name}({parameters1}) {{");
    }

    private void AppendBody(StringBuilder stringBuilder)
    {
        // Todo to implementation
    }


    private static IReadOnlyCollection<HttpMethodType> GetMethodTypes(string methodTypes)
    {
        IEnumerable<string> splitMethodTypes = methodTypes.Replace("[", "").Replace("]", "").Split(",").Select(x => x.Trim());

        return splitMethodTypes
            .Select(HttpMethodTypeExtensionMethods.GetHttpMethodType)
            .Where(x => x is not null)
            .Select(x => x!.Value)
            .ToList()
            .AsReadOnly();
    }
}