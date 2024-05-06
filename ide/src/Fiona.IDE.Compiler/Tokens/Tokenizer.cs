using Fiona.IDE.Compiler.Tokens.Exceptons;

namespace Fiona.IDE.Compiler.Tokens;

internal static class Tokenizer
{
    private static readonly char[] SplitChars = [';'];

    public static Task<IReadOnlyCollection<IToken>> GetTokensAsync(StreamReader input)
    {
        if (input is null)
        {
            throw new EmptyInputStreamException();
        }

        return ReadTokensFromInputAsync(input);
    }

    private static async Task<IReadOnlyCollection<IToken>> ReadTokensFromInputAsync(StreamReader input)
    {
        List<IToken> tokens = [];
        while (!input.EndOfStream)
        {
            IEnumerable<string> commands = await GetCommandsFromNextLineAsync(input);
            tokens.AddRange(commands.Select(TokenFactory.CreateToken));
        }

        return tokens;
    }

    private static async Task<IEnumerable<string>> GetCommandsFromNextLineAsync(TextReader input)
    {
        string? inputLine = await input.ReadLineAsync();
        if (inputLine is null)
        {
            throw new Exception("Something went wrong while read file");
        }
        List<string> splitLine = inputLine.Split(SplitChars, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList();

        return splitLine;
    }
}