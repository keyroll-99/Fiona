namespace Fiona.IDE.Tokenizer;

internal sealed class Token : IToken
{
    public string? Value { get; }
    public string[]? ArrayOfValues { get; }
    public TokenType Type { get; }
    

    public Token(TokenType type) : this(type, type.ToString())
    {
    }
    public Token(TokenType type, string value)
    {
        Value = value;
        Type = type;
        ArrayOfValues = [value];
    }

    public Token(TokenType type, params string[] arrayOfValue)
    {
        ArrayOfValues = arrayOfValue;
        Type = type;
    }
}