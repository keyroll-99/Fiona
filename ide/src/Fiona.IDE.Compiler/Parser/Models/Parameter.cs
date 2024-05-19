using Fiona.IDE.Compiler.Parser.Exceptions;
using Fiona.IDE.Compiler.Tokens;

namespace Fiona.IDE.Compiler.Parser.Models;

internal sealed class Parameter
{
    public ParameterType Type { get; }
    public string Name => _name;

    private readonly string _attribute;
    private readonly string _type;
    private readonly string _name;

    private Parameter(string attribute, string type, string name)
    {
        _attribute = attribute;
        _type = type;
        _name = name;

        Type = attribute switch
        {
            "[Body]" => ParameterType.Body,
            "[QueryParam]" => ParameterType.Query,
            "[FromRoute]" => ParameterType.Path,
            "[Cookie]" => ParameterType.Cookie,
            _ => throw new ValidationError("Invalid parameter attribute")
        };
    }

    public string GenerateSourceCode()
    {
        return $"{_attribute} {_type} {_name}";
    }

    public static List<Parameter> GetParametersFromToken(IToken token)
    {
        List<Parameter> result = [];

        foreach (string parameter in token.ArrayOfValues ?? [])
        {
            (string? parameterDeclaration, string? parameterType) = parameter.Split(":") switch
            {
                { Length: 2 } array => (array[0], array[1]),
                _ => throw new ValidationError("Invalid parameter declaration")
            };

            (string? parameterAttribute, string parameterName) = parameterDeclaration.Split(" ") switch
            {
                { Length: 2 } array => (array[0], array[1]),
                _ => throw new ValidationError("Invalid parameter declaration")
            };

            result.Add(new Parameter(parameterAttribute, parameterType, parameterName));
        }

        return result;
    }

}