using Fiona.IDE.Compiler.Parser.Exceptions;
using Fiona.IDE.Compiler.Tokens;

namespace Fiona.IDE.Compiler.Parser;

internal sealed class Validator
{
    
    public static async Task ValidateAsync(IReadOnlyCollection<IToken> tokens)
    {
        await Task.CompletedTask;
        bool foundUsing = false;
        for (int i = 0; i < tokens.Count; i++)
        {
            IToken currentElement = tokens.ElementAt(i);
            switch (currentElement.Type)
            {
                case TokenType.UsingBegin:
                    if (foundUsing)
                    {
                        throw new ValidationError("Duplicate using statement.");
                    }
                    i = ValidateUsing(tokens, i + 1);
                    foundUsing = true;
                    continue;
                case TokenType.Class:
                    i = ValidateClass(tokens, i + 1);
                    continue;
                case TokenType.UsingEnd:
                case TokenType.Using:
                case TokenType.Route:
                case TokenType.Endpoint:
                case TokenType.EndpointEnd:
                case TokenType.BodyBegin:
                case TokenType.BodyEnd:
                case TokenType.Comment:
                case TokenType.Method:
                default:
                    throw new ValidationError("Invalid order of code.");
            }
        }
    }

    private static int ValidateUsing(IReadOnlyCollection<IToken> tokens, int startIndex)
    {
        for (int i = startIndex; i < tokens.Count; i++)
        {
            IToken currentToken = tokens.ElementAt(i);
            if (currentToken.Type == TokenType.UsingEnd)
            {
                return i;
            }
            if (currentToken.Type != TokenType.Using)
            {
                throw new ValidationError($"In the using statement was found {currentToken.Type.ToString()}");
            }
        }
        throw new ValidationError("Cannot validate using statement.");
    }
    
    private static int ValidateClass(IReadOnlyCollection<IToken> tokens, int startIndex)
    {
        return tokens.Count;
    }
    
}