using Fiona.Compiler.Parser;
using Fiona.Compiler.Parser.Builders;
using Fiona.Compiler.Tokenizer;

namespace Fiona.Compiler;

internal sealed class Compiler(IParser parser) : ICompiler
{

    public async Task CompileFile(ProjectFile projectFile)
    {
        await ParseFileAsync(projectFile);
    }

    public async Task RunAsync(IEnumerable<ProjectFile> projectFiles)
    {
        List<Task> parsingTask = projectFiles.Select(ParseFileAsync).ToList();
        await Task.WhenAll(parsingTask);
    }


    private async Task ParseFileAsync(ProjectFile projectFile)
    {
        await using FileStream file = File.Open(projectFile.Path, FileMode.Open);
        using StreamReader reader = new(file);
        IReadOnlyCollection<IToken> tokens = await Tokenizer.Tokenizer.GetTokensAsync(reader);
        ReadOnlyMemory<byte> parsedContent = await parser.ParseAsync(tokens, projectFile);

        await using FileStream csFile = File.Open($"{projectFile.Path}.cs", FileMode.Create);
        await csFile.WriteAsync(parsedContent);
    }
}