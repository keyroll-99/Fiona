using Fiona.Compiler.Tokenizer;

namespace Fiona.Compiler.ProjectManager.Models;

public sealed class Dependency(string name, string type)
{
    public string Name { get; } = name;
    public string Type { get; } = type;


    public static List<Dependency> GetDependenciesFromToken(IToken token)
    {
        List<Dependency> result = [];
        foreach (string dependency in token.ArrayOfValues ?? [])
        {
            (string name, string type) = dependency.Split(":") switch
            {
                { Length: 2 } array => (array[0], array[1]),
                _ => throw new Exception("Invalid dependency declaration")
            };

            result.Add(new Dependency(name, type));
        }

        return result;
    }

    public override string ToString() => $"{name}: {type}";
}