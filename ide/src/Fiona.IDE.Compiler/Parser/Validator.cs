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
                    i = ValidateClass(tokens, i + 1, currentElement);
                    continue;
                default:
                    throw new ValidationError($"Cannot use {currentElement.Type.GetTokenKeyword()} out of class definition.");
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
        throw new ValidationError("Not found end of using statement.");
    }

    private static int ValidateClass(IReadOnlyCollection<IToken> tokens, int startIndex, IToken classToken)
    {
        bool isRoutingDefine = false;
        bool isDiDefine = false;

        for (int i = startIndex; i < tokens.Count; i++)
        {
            IToken currentToken = tokens.ElementAt(i);
            switch (currentToken.Type)
            {
                case TokenType.Route:
                    if (isRoutingDefine)
                    {
                        throw new ValidationError("Duplicate route definition.");
                    }
                    isRoutingDefine = true;
                    continue;
                case TokenType.Endpoint:
                    i = ValidateEndpoint(tokens, i + 1, currentToken.Value);
                    continue;
                case TokenType.Dependency:
                    if (isDiDefine)
                    {
                        throw new ValidationError($"Parameter type in {classToken.Value} is define two times");
                    }
                    isDiDefine = true;
                    continue;
                default:
                    throw new ValidationError($"Cannot use {currentToken.Type.GetTokenKeyword()} in class definition.");
            }

        }
        return tokens.Count;
    }

    private static int ValidateEndpoint(IReadOnlyCollection<IToken> tokens, int startIndex, string endpointName)
    {
        bool isMethodDefine = false;
        bool isRouteDefine = false;
        bool isReturnDefine = false;
        bool isInputDefine = false;
        for (int i = startIndex; i < tokens.Count; i++)
        {
            IToken currentToken = tokens.ElementAt(i);
            switch (currentToken.Type)
            {
                case TokenType.Method:
                    if (isMethodDefine)
                    {
                        throw new ValidationError($"Method in {endpointName} is define two times");
                    }
                    isMethodDefine = true;
                    continue;
                case TokenType.Route:
                    if (isRouteDefine)
                    {
                        throw new ValidationError($"Route in {endpointName} is define two times");
                    }
                    isRouteDefine = true;
                    continue;
                case TokenType.ReturnType:
                    if (isReturnDefine)
                    {
                        throw new ValidationError($"Return type in {endpointName} is define two times");
                    }
                    isReturnDefine = true;
                    continue;
                case TokenType.Parameter:
                    if (isInputDefine)
                    {
                        throw new ValidationError($"Parameter type in {endpointName} is define two times");
                    }
                    ValidateParameter(currentToken);
                    isInputDefine = true;
                    continue;
                case TokenType.BodyBegin:
                    return ValidateMethodBody(tokens, i + 1, endpointName);
                default:
                    throw new ValidationError($"Cannot use {currentToken.Type.GetTokenKeyword()} in endpoint declaration");
            }

        }

        return tokens.Count;
    }

    private static readonly IReadOnlyCollection<string> AvailableParameterTypes =
    [
        "FromRoute", "QueryParam", "Body", "Cookie"
    ];

    private static void ValidateParameter(IToken parameterToken)
    {
        foreach (string parameter in parameterToken.ArrayOfValues ?? [])
        {
            (string? parameterDeclaration, string? parameterType) = parameter.Split(":") switch
            {
                { Length: 2 } array => (array[0], array[1]),
                _ => throw new ValidationError("Invalid parameter declaration")
            };
            
            (string? parameterAttribute, string parameterName) = parameterDeclaration.Split(" ") switch
            {
                { Length: 2 } array => (array[0], array[1]),
                _ => throw new ValidationError("Invalid parameter declaration")
            };
            
            parameterAttribute = parameterAttribute.Replace("[", "").Replace("]", "");
            if (!AvailableParameterTypes.Contains(parameterAttribute))
            {
                throw new ValidationError($"Invalid parameter type {parameterAttribute}");
            }
        }
    }

    private static int ValidateMethodBody(IReadOnlyCollection<IToken> tokens, int startIndex, string endpointName)
    {
        for (int i = startIndex; i < tokens.Count; i++)
        {
            IToken currentToken = tokens.ElementAt(i);
            switch (currentToken.Type)
            {
                case TokenType.Comment:
                    continue;
                case TokenType.BodyEnd:
                    return i;
                default:
                    throw new ValidationError($"Cannot use {currentToken.Type.GetTokenKeyword()} in body");
            }
        }

        throw new ValidationError($"Not found end of body in {endpointName}");
    }

}