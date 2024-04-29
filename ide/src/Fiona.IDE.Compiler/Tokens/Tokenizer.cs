namespace Fiona.IDE.Compiler.Tokens;

internal static class Tokenizer
{
    public static async Task<IReadOnlyCollection<IToken>> GetTokensAsync(StreamReader input)
    {
        List<IToken> tokens = [];
        if (input is null)
        {
            throw new Exception("TODO: custom exception");// Todo custom exception
        }
        bool isUsing = false;
        while (!input.EndOfStream)
        {
            string? line = await input.ReadLineAsync();
            if (line is null)
            {
                throw new Exception("TODO: custom exception");// Todo custom exception
            }
            if (string.Equals(line.Trim(), TokenType.UsingBegin.GetTokenKeyword(), StringComparison.CurrentCultureIgnoreCase))
            {
                isUsing = true;
                tokens.Add(new Token(TokenType.UsingBegin));
                continue;
            }
            if (string.Equals(line.Trim(), TokenType.UsingEnd.GetTokenKeyword(), StringComparison.CurrentCultureIgnoreCase))
            {
                tokens.Add(new Token(TokenType.UsingEnd));
                isUsing = false;
                continue;
            }
            if (isUsing)
            {
                tokens.Add(new Token(TokenType.Using, line.Trim()));
                continue;
            }

            if (line.Trim().StartsWith(TokenType.Class.GetTokenKeyword()))
            {
                tokens.Add(new Token(TokenType.Class, line[..TokenType.Class.GetTokenKeyword().Length].Trim()));
            }
            
        }

        return tokens;
    }
}