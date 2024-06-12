using Fiona.Compiler.Parser.Exceptions;
using Fiona.Compiler.Tokenizer;

namespace Fiona.Compiler.Parser.Models;

internal sealed class Dependency
{

    private Dependency(string name, string type)
    {
        Name = name;
        Type = type;
    }
    public string Name { get; }
    public string Type { get; }

    public string FullDeclaration => $"{Type} {Name}";

    public static List<Dependency> GetDependenciesFromToken(IToken token)
    {
        List<Dependency> result = [];
        foreach (string dependency in token.ArrayOfValues ?? [])
        {
            (string name, string type) = dependency.Split(":") switch
            {
                { Length: 2 } array => (array[0], array[1]),
                _ => throw new ValidationError("Invalid dependency declaration")
            };

            result.Add(new Dependency(name, type));
        }

        return result;
    }
}