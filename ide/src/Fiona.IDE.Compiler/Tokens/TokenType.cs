namespace Fiona.IDE.Compiler.Tokens;

internal enum TokenType
{
    UsingBegin,
    UsingEnd,
    Using,
    Route,
    Endpoint,
    EndpointEnd,
    BodyBegin,
    BodyEnd,
    Comment,
    Class,
    ClassEnd,
    Method,
}


internal static  class TokenTypeExtension {

    public static string GetTokenKeyword(this TokenType tokenType)
    {
        return tokenType switch
        {
            TokenType.UsingBegin => "usingBegin;",
            TokenType.UsingEnd => "usingEnd;",
            TokenType.Using => string.Empty,
            TokenType.Class => "class",
            TokenType.Route => "route:",
            _ => throw new ArgumentOutOfRangeException(nameof(tokenType), tokenType, null)
        };
    }
}