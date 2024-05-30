namespace Fiona.IDE.Tokenizer;

public enum TokenType
{
    UsingBegin,
    UsingEnd,
    Using,
    Route,
    Endpoint,
    BodyBegin,
    BodyEnd,
    Comment,
    Class,
    Method,
    ReturnType,
    Parameter,
    Dependency,
    Namespace,
    Body,
}


public static  class TokenTypeExtension {

    public static string GetTokenKeyword(this TokenType tokenType)
    {
        return tokenType switch
        {
            TokenType.UsingBegin => "usingBegin",
            TokenType.UsingEnd => "usingEnd",
            TokenType.Using => "using",
            TokenType.Route => "route:",
            TokenType.Endpoint => "endpoint:",
            TokenType.BodyBegin => "bodyBegin",
            TokenType.BodyEnd => "bodyEnd",
            TokenType.Comment => "//",
            TokenType.Class => "class",
            TokenType.Method => "method:",
            TokenType.ReturnType => "return:",
            TokenType.Parameter => "input:",
            TokenType.Dependency => "inject:",
            TokenType.Namespace => "namespace:",
            TokenType.Body => "",
            _ => throw new ArgumentOutOfRangeException(nameof(tokenType), tokenType, null)
        };
    }
}