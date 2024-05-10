using Fiona.IDE.Compiler.Tokens;

namespace Fiona.IDE.Compiler.Parser;

internal sealed class Validator
{
    public async Task<bool> ValidateAsync(IEnumerable<IToken> tokens)
    {
        return ValidateUsing(tokens);
    }

    private bool ValidateUsing(IEnumerable<IToken> tokens)
    {
        return true;
    }
    
}