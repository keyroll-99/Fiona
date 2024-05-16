using Fiona.IDE.Compiler.Tokens;
using System.Text;

namespace Fiona.IDE.Compiler.Parser.Models;

internal sealed class Endpoint(string name, string? route, string? methodTypes, string? returnType, IReadOnlyCollection<IToken> bodyTokens)
{
    private readonly IReadOnlyCollection<HttpMethodType> _methodTypes = GetMethodTypes(methodTypes);
    private readonly IReadOnlyCollection<IToken> _bodyTokens = bodyTokens;

    public string BuildSourceCode()
    {
        StringBuilder sourceCode = new StringBuilder(1000);

        string methodTypes = string.Join(" | ", _methodTypes.Select(x => x.GetMethodTypesUsing()));
        string routeValue = route is not null ? $", \"{route}\"" : "";
        sourceCode.Append($"[Route({methodTypes}{routeValue})]\n");
        AppendMethodDeclaration(sourceCode);
        AppendBody(sourceCode);
        sourceCode.Append("}");

        return sourceCode.ToString();
    }
    
    private void AppendMethodDeclaration(StringBuilder sourceCode)
    {
        sourceCode.Append("public async Task");
        if (returnType is not null)
        {
            sourceCode.Append($"<{returnType}>");
        }
        
        sourceCode.Append($" {name}() {{");
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