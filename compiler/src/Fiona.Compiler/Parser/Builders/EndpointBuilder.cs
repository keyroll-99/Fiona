using Fiona.Compiler.Tokenizer;
using System.Text;

namespace Fiona.Compiler.Parser.Builders;

internal sealed class EndpointBuilder(string name, string? route, string? methodTypes, string? returnType, IReadOnlyCollection<ParameterBuilder> parameters, IToken? bodyToken)
{
    private readonly IReadOnlyCollection<HttpMethodTypeBuilder> _methodTypes = GetMethodTypes(methodTypes);

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
        IEnumerable<ParameterBuilder> queryParameters = parameters.Where(x => x.Type == ParameterType.Query).ToList();
        string routeParameters = queryParameters.Any() ? $", [{string.Join(", ", queryParameters.Select(x => $"\"{x.Name}\""))}]" : string.Empty;

        sourceCode.Append($"[Route({methodTypes}{routeValue}{routeParameters})]\n");
    }

    private void AppendMethodDeclaration(StringBuilder sourceCode)
    {
        sourceCode.Append("public async Task");
        if (returnType is not null)
        {
            sourceCode.Append($"<{returnType}>");
        }

        string methodParameters = string.Join(", ", parameters.Select(x => x.GenerateSourceCode()));

        sourceCode.Append($" {name}({methodParameters}) {{");
    }

    private void AppendBody(StringBuilder sourceCode)
    {
        if (bodyToken is null)
        {
            return;
        }
        sourceCode.Append(bodyToken.Value);
    }


    private static IReadOnlyCollection<HttpMethodTypeBuilder> GetMethodTypes(string? methodTypes)
    {
        methodTypes ??= "[GET]";
        IEnumerable<string> splitMethodTypes = methodTypes.Replace("[", "").Replace("]", "").Split(",").Select(x => x.Trim());

        return splitMethodTypes
            .Select(HttpMethodTypeExtensionMethods.GetHttpMethodType)
            .Where(x => x is not null)
            .Select(x => x!.Value)
            .ToList()
            .AsReadOnly();
    }
}