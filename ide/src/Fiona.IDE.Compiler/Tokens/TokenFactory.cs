using Fiona.IDE.Compiler.Parser.Exceptions;

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
        },
        {
            TokenType.ReturnType, GetReturnToken
        },
        {
            TokenType.Parameter, GetParameterToken
        },
        {
            TokenType.Dependency, GetDependencyToken
        }
    };

    private static readonly Dictionary<TokenType, string> TokenKeywords = new();

    static TokenFactory()
    {
        foreach (TokenType tokenType in Enum.GetValues<TokenType>())
        {
            TokenKeywords[tokenType] = tokenType.GetTokenKeyword();
        }
    }

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

        throw new ValidationError("Cannot parse file");
    }

    private static IEnumerable<TokenType> GetTokenTypesToCheck(string input)
        => Enum.GetValues<TokenType>().Where(tokenType => input.Contains(TokenKeywords[tokenType]));

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

    private static IToken? GetBodyBeginToken(string command)
        => GetTokenEquals(command, TokenType.BodyBegin);

    private static IToken? GetBodyEndToken(string command)
        => GetTokenEquals(command, TokenType.BodyEnd);

    private static IToken? GetCommentToken(string command)
        => GetTokenStartWith(command, TokenType.Comment);

    private static IToken? GetClassToken(string command)
        => GetTokenStartWith(command, TokenType.Class);

    private static IToken? GetMethodToken(string command)
        => GetTokenStartWith(command, TokenType.Method); // TODO It should behave like GetParameterToken

    private static IToken? GetReturnToken(string command)
        => GetTokenStartWith(command, TokenType.ReturnType);

    private static IToken? GetParameterToken(string command)
        => GetArrayToken(command, TokenType.Parameter);

    private static IToken? GetDependencyToken(string command)
        => GetArrayToken(command, TokenType.Dependency);

    private static Token? GetTokenStartWith(string command, TokenType tokenType, bool hasValue = true)
    {
        string keyword = TokenKeywords[tokenType];
        if (command.StartsWith(keyword, StringComparison.CurrentCultureIgnoreCase))
        {
            return hasValue ? new Token(tokenType, command[keyword.Length..].Trim()) : new Token(tokenType);
        }
        return null;
    }

    private static Token? GetTokenEquals(string command, TokenType tokenType)
    {
        return string.Equals(command.Trim(), TokenKeywords[tokenType], StringComparison.CurrentCultureIgnoreCase) ? new Token(tokenType) : null;
    }

    private static Token? GetArrayToken(string command, TokenType tokenType)
    {
        string keyword = TokenKeywords[tokenType];
        if (!command.StartsWith(keyword, StringComparison.CurrentCultureIgnoreCase))
        {
            return null;
        }
        
        string[] parameters = command[keyword.Length..]
            .Trim()
            .Split('-')
            .Select(x => x.Trim())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToArray();
        return new Token(tokenType, parameters);
    }
}