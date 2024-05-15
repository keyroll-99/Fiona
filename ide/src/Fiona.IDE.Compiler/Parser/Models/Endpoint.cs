using Fiona.IDE.Compiler.Tokens;

namespace Fiona.IDE.Compiler.Parser.Models;

internal sealed class Endpoint(string name, string? route, string? methodTypes, IReadOnlyCollection<IToken> bodyTokens)
{
    private readonly string _name = name;
    private readonly string? _route = route;
    private readonly IReadOnlyCollection<HttpMethodType> _methodTypes = GetMethodTypes(methodTypes);
    private readonly IReadOnlyCollection<IToken> _bodyTokens = bodyTokens;


    public string BuildSourceCode()
    {
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