using Fiona.Compiler.Parser.Exceptions;
using Fiona.Compiler.Tokenizer;

namespace Fiona.Compiler.Parser.Builders;

internal sealed class DependencyBuilder
{

    private DependencyBuilder(string name, string type)
    {
        Name = name;
        Type = type;
    }
    public string Name { get; }
    public string Type { get; }

    public string FullDeclaration => $"{Type} {Name}";

    public static List<DependencyBuilder> GetDependenciesFromToken(IToken token)
    {
        List<DependencyBuilder> result = [];
        foreach (string dependency in token.ArrayOfValues ?? [])
        {
            (string name, string type) = dependency.Split(":") switch
            {
                { Length: 2 } array => (array[0], array[1]),
                _ => throw new ValidationError("Invalid dependency declaration")
            };

            result.Add(new DependencyBuilder(name, type));
        }

        return result;
    }
}