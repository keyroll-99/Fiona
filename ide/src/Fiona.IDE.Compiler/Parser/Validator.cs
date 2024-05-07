using Fiona.IDE.Compiler.Tokens;

namespace Fiona.IDE.Compiler.Parser;

internal sealed class Validator
{
    public Task<bool> ValidateAsync(IEnumerable<IToken> tokens)
    {
        return Task.FromResult(true);

    }
}