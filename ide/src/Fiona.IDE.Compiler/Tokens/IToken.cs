namespace Fiona.IDE.Compiler.Tokens;

internal interface IToken
{
    public string Value { get; }
    public TokenType Type { get; }
}