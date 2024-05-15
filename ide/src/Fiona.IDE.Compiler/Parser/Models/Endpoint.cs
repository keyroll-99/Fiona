using Fiona.IDE.Compiler.Tokens;
using System.Text;

namespace Fiona.IDE.Compiler.Parser.Models;

internal sealed class Endpoint(string name, string? route, string? methodTypes, IReadOnlyCollection<IToken> bodyTokens)
{
    private readonly IReadOnlyCollection<HttpMethodType> _methodTypes = GetMethodTypes(methodTypes);
    private readonly IReadOnlyCollection<IToken> _bodyTokens = bodyTokens;

    public string BuildSourceCode()
    {
        StringBuilder sourceCode = new StringBuilder(1000);

        string methodTypes = string.Join(" | ", _methodTypes.Select(x => x.GetMethodTypesUsing()));
        string route1 = route is not null ? $", \"{route}\"" : "";
        sourceCode.Append($"[Route({methodTypes}{route1})]\n");
        sourceCode.Append($"public async Task {name}()\n {{");
        AppendBody(sourceCode);
        sourceCode.Append("}");

        return sourceCode.ToString();
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