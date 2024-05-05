namespace Fiona.IDE.Compiler.Tokens;

internal static class TokenFactory
{
    private static readonly Dictionary<TokenType, Func<string, IToken?>> TokenVerify = new()
    {

        {
            TokenType.UsingBegin, GetStartUsingToken
        },
        {
            TokenType.UsingEnd, GetEndUsingToken
        },
        {
            TokenType.Using, GetUsingToken
        },
        {
            TokenType.Route, GetRouteToken
        },
        {
            TokenType.Endpoint, GetEndpointToken
        },
        {
            TokenType.EndpointEnd, GetEndpointEndToken
        },
        {
            TokenType.BodyBegin, GetBodyBeginToken
        },
        {
            TokenType.BodyEnd, GetBodyEndToken
        },
        {
            TokenType.Comment, GetCommentToken
        },
        {
            TokenType.Class, GetClassToken
        },
        {
            TokenType.Method, GetMethodToken
        }
    };


    public static IToken CreateToken(string command)
    {
        IEnumerable<TokenType> tokenToCheck = GetTokenTypesToCheck(command);
        foreach (TokenType tokenType in tokenToCheck)
        {
            IToken? token = TokenVerify[tokenType](command);
            if (token is not null)
            {
                return token;
            }
        }

        throw new Exception("todo");
    }

    private static IEnumerable<TokenType> GetTokenTypesToCheck(string input)
        => Enum.GetValues<TokenType>().Where(tokenType => input.Contains(tokenType.GetTokenKeyword()));

    private static IToken? GetStartUsingToken(string command)
        => GetTokenEquals(command, TokenType.UsingBegin);

    private static IToken? GetEndUsingToken(string command)
        => GetTokenEquals(command, TokenType.UsingEnd);

    private static IToken? GetUsingToken(string command)
        => GetTokenStartWith(command, TokenType.Using);

    private static IToken? GetRouteToken(string command)
        => GetTokenStartWith(command, TokenType.Route);

    private static IToken? GetEndpointToken(string command)
        => GetTokenStartWith(command, TokenType.Endpoint);

    private static IToken? GetEndpointEndToken(string command)
        => GetTokenEquals(command, TokenType.EndpointEnd);

    private static IToken? GetBodyBeginToken(string command)
        => GetTokenEquals(command, TokenType.BodyBegin);

    private static IToken? GetBodyEndToken(string command)
        => GetTokenEquals(command, TokenType.BodyEnd);

    private static IToken? GetCommentToken(string command)
        => GetTokenStartWith(command, TokenType.Comment);

    private static IToken? GetClassToken(string command)
        => GetTokenStartWith(command, TokenType.Class);

    private static IToken? GetMethodToken(string command)
        => GetTokenStartWith(command, TokenType.Method);

    private static IToken? GetTokenStartWith(string command, TokenType tokenType, bool hasValue = true)
    {
        string keyword = tokenType.GetTokenKeyword();
        if (command.StartsWith(keyword, StringComparison.CurrentCultureIgnoreCase))
        {
            return hasValue ? new Token(tokenType, command[keyword.Length..].Trim()) : new Token(tokenType);
        }
        return null;
    }

    private static IToken? GetTokenEquals(string command, TokenType tokenType)
    {
        return
            string.Equals(command.Trim(), tokenType.GetTokenKeyword(), StringComparison.CurrentCultureIgnoreCase) ? new Token(tokenType) : null;
    }
}