namespace Fiona.IDE.Compiler.Tokens;

internal interface IToken
{
    public string? Value { get; }
    public TokenType Type { get; }
    public string[]? ArrayOfValues { get; }
}