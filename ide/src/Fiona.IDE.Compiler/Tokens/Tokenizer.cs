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
                continue;
            }
            
            if (line.Trim().StartsWith(TokenType.Route.GetTokenKeyword()))
            {
                tokens.Add(new Token(TokenType.Route, line[..TokenType.Route.GetTokenKeyword().Length].Trim()));
                continue;
            }
            
            if (line.Trim().StartsWith(TokenType.Endpoint.GetTokenKeyword()))
            {
                tokens.Add(new Token(TokenType.Endpoint, line[..TokenType.Endpoint.GetTokenKeyword().Length].Trim()));
                continue;
            }
            
            if (line.Trim().StartsWith(TokenType.Method.GetTokenKeyword()))
            {
                tokens.Add(new Token(TokenType.Method, line[..TokenType.Method.GetTokenKeyword().Length].Trim()));
                continue;
            }
            
            if (line.Trim().StartsWith(TokenType.BodyBegin.GetTokenKeyword()))
            {
                tokens.Add(new Token(TokenType.BodyBegin));
                continue;
            }
            
            if (line.Trim().StartsWith(TokenType.BodyEnd.GetTokenKeyword()))
            {
                tokens.Add(new Token(TokenType.BodyEnd));
                continue;
            }
            
            if (line.Trim().StartsWith(TokenType.Comment.GetTokenKeyword()))
            {
                tokens.Add(new Token(TokenType.Comment, line[..TokenType.Comment.GetTokenKeyword().Length].Trim()));
                continue;
            }  
            
            if (line.Trim().StartsWith(TokenType.ClassEnd.GetTokenKeyword()))
            {
                tokens.Add(new Token(TokenType.ClassEnd));
                continue;
            }
            
            if (line.Trim().StartsWith(TokenType.EndpointEnd.GetTokenKeyword()))
            {
                tokens.Add(new Token(TokenType.EndpointEnd));
            }
            
        }

        return tokens;
    }
    
}