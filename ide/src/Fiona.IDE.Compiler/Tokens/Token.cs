namespace Fiona.IDE.Compiler.Tokens;

internal sealed class Token : IToken
{
    public string? Value { get; }
    public string[]? ArrayOfValue { get; }
    public TokenType Type { get; }

    public Token(TokenType type) : this(type, type.ToString())
    {
    }
    public Token(TokenType type, string value)
    {
        Value = value;
        Type = type;
    }
    
    public Token(TokenType type, params string[] arrayOfValue)
    {
        ArrayOfValue = arrayOfValue;
        Type = type;
    }
}