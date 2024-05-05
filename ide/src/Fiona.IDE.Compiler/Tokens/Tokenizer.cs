using System.Collections;

namespace Fiona.IDE.Compiler.Tokens;

internal static class Tokenizer
{
    private const string SplitChar = ";";
    
    public static Task<IReadOnlyCollection<IToken>> GetTokensAsync(StreamReader input)
    {
        if (input is null)
        {
            throw new Exception("TODO: custom exception");// Todo custom exception
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

    private static async Task<IEnumerable<string>> GetCommandsFromNextLineAsync(StreamReader input)
    {
        string? inputLine = await input.ReadLineAsync();
        if (inputLine is null)
        {
            throw new Exception("TODO: custom exception");// Todo custom exception
        }
        List<string> splitLine = inputLine.Split(SplitChar).ToList();

        return splitLine.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim());
    }

    private static async Task<IEnumerable<Token>> GetClassTokens(StreamReader input)
    {
        List<Token> tokens = [];

        while (!input.EndOfStream)
        {
            string? line = await input.ReadLineAsync();
            if (line is null)
            {
                throw new Exception("TODO: custom exception");// Todo custom exception
            }
            // todo: throw exception when find other token before class
            
            if (line.Trim().StartsWith(TokenType.Class.GetTokenKeyword(),StringComparison.CurrentCultureIgnoreCase))
            {
                tokens.Add(new Token(TokenType.Class, line[..TokenType.Class.GetTokenKeyword().Length].Trim()));
                continue;
            }
            
            if(line.Trim().StartsWith(TokenType.Route.GetTokenKeyword(), StringComparison.CurrentCultureIgnoreCase))
            {
                tokens.Add(new Token(TokenType.Route, line[..TokenType.Route.GetTokenKeyword().Length].Trim()));
            }

            if (line.Trim().StartsWith(TokenType.Endpoint.GetTokenKeyword(), StringComparison.CurrentCultureIgnoreCase))
            {
                input.BaseStream.Seek(-line.Length, SeekOrigin.Current);
            }
        }

        return tokens;
    }

}
