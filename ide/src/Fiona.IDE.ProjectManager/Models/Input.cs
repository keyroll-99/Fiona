using Fiona.IDE.Tokenizer;

namespace Fiona.IDE.ProjectManager.Models;

public sealed class Input
{
    private string _name;
    private string _type;
    private string _attribute;

    private Input(string name, string type, string attribute)
    {
        _name = name;
        _type = type;
        _attribute = attribute;
    }

    public static List<Input> GetInputsFromToken(IToken token)
    {
        List<Input> result = [];
        foreach (string parameter in token.ArrayOfValues ?? [])
        {
            (string? parameterDeclaration, string? parameterType) = parameter.Split(":") switch
            {
                { Length: 2 } array => (array[0], array[1]),
                _ => throw new Exception("Invalid parameter declaration")
            };

            (string? parameterAttribute, string parameterName) = parameterDeclaration.Split(" ") switch
            {
                { Length: 2 } array => (array[0], array[1]),
                _ => throw new Exception("Invalid parameter declaration")
            };

            result.Add(new Input(parameterAttribute, parameterType, parameterName));
        }
        return result;
    }

    public override string ToString() =>
        $" [{_attribute}] {_name}: {_type}";
}