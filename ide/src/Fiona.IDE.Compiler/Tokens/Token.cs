namespace Fiona.IDE.Compiler.Tokens;

internal sealed class Token(string value, TokenType type) : IToken
{

    public string Value { get; } = value;
    public TokenType Type { get; } = type;

}