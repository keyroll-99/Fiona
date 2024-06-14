using Fiona.Compiler.Parser.Exceptions;
using Fiona.Compiler.Tokenizer;

namespace Fiona.Compiler.Parser.Builders;

internal sealed class ParameterBuilder
{

    private readonly string _attribute;
    private readonly string _type;

    private ParameterBuilder(string attribute, string type, string name)
    {
        _type = type;
        Name = name;

        Type = attribute switch
        {
            "[Body]" => ParameterType.Body,
            "[QueryParam]" => ParameterType.Query,
            "[FromRoute]" => ParameterType.Path,
            "[Cookie]" => ParameterType.Cookie,
            _ => throw new ValidationError("Invalid parameter attribute")
        };
        _attribute = attribute;
        if (Type == ParameterType.Path)
        {
            _attribute = string.Empty;// path doesn't have own attribute
        }
    }
    public ParameterType Type { get; }
    public string Name { get; }

    public string GenerateSourceCode() => $"{_attribute} {_type} {Name}";

    public static List<ParameterBuilder> GetParametersFromToken(IToken token)
    {
        List<ParameterBuilder> result = [];

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

            result.Add(new ParameterBuilder(parameterAttribute, parameterType, parameterName));
        }

        return result;
    }
}