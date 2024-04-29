namespace Fiona.IDE.Compiler.Tokens;

internal sealed class Token(TokenType type, string value) : IToken
{
    public string Value { get; } = value;
    public TokenType Type { get; } = type;

    public Token(TokenType type) : this(type, type.ToString())
    {
    }
}
