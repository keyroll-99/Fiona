using Fiona.IDE.Compiler.Parser.Exceptions;
using Fiona.IDE.Compiler.Tokens;

namespace Fiona.IDE.Compiler.Parser;

internal sealed class Validator
{
    public async Task ValidateAsync(IReadOnlyCollection<IToken> tokens)
    {
        for (int i = 0; i < tokens.Count; i++)
        {
            if (tokens.ElementAt(i).Type == TokenType.UsingBegin)
            {
                i = ValidateUsing(tokens, i + 1);
            }            
        }
    }


    private int ValidateUsing(IReadOnlyCollection<IToken> tokens, int startIndex)
    {
        for (int i = startIndex; startIndex < tokens.Count; startIndex++)
        {
            
        }
        throw new ValidationError("Cannot validate using statement.");
    }

    
}