namespace Fiona.IDE.Compiler.Tokens;

internal static class TokenFactory
{
    
    public static IToken CreateToken(string command)
    {
        IToken? token = GetUsingToken(command);
        if (token is not null)
        {
            return token;
        }
        
        return token;
    }


    private static IToken? GetUsingToken(string command)
    {
        if (string.Equals(command.Trim(), TokenType.UsingBegin.GetTokenKeyword(), StringComparison.CurrentCultureIgnoreCase))
        {
            return new Token(TokenType.UsingBegin);
        }
        if (string.Equals(command.Trim(), TokenType.UsingEnd.GetTokenKeyword(), StringComparison.CurrentCultureIgnoreCase))
        {
            return new Token(TokenType.UsingEnd);
        }
        if (command.StartsWith(TokenType.Using.GetTokenKeyword(), StringComparison.CurrentCultureIgnoreCase))
        {
           return new Token(TokenType.Using, $"{command};");
        }
        return null;
    }
    
    private static IToken? GetClassToken(string command)
    {
        if (string.Equals(command.Trim(), TokenType.Class.GetTokenKeyword(), StringComparison.CurrentCultureIgnoreCase))
        {
            return new Token(TokenType.Class);
        }
        return null;
    }
}