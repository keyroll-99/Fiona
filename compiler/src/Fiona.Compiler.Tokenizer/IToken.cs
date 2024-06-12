namespace Fiona.Compiler.Tokenizer;

public interface IToken
{
    public string? Value { get; }
    public TokenType Type { get; }
    public string[]? ArrayOfValues { get; }
}
