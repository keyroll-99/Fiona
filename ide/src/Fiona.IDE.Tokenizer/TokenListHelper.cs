namespace Fiona.IDE.Tokenizer;

public static class TokenListHelper
{
    public static (List<IToken> tokens, int endSearchIndex) GetUsingTokens(this IReadOnlyCollection<IToken> tokens, int startSearchIndex = 0)
    {
        List<IToken> result = [];
        bool isUsingPart = false;
        for (int i = startSearchIndex; i < tokens.Count; i++)
        {
            IToken currentToken = tokens.ElementAt(i);
            switch (currentToken.Type)
            {
                case TokenType.UsingBegin:
                    isUsingPart = true;
                    break;
                case TokenType.UsingEnd:
                    return (result, i);
                case TokenType.Using:
                    if (isUsingPart)
                    {
                        result.Add(currentToken);
                    }
                    break;
                case TokenType.Comment:
                    continue;
            }
        }

        return (result, tokens.Count);
    }

    public static IToken? GetNamespaceToken(this IReadOnlyCollection<IToken> tokens)
        => tokens.FirstOrDefault(x => x.Type == TokenType.Namespace);
    
    public static (IToken classToken, int findIndex) GetClassToken(this IReadOnlyCollection<IToken> tokens, int startSearchIndex = 0)
    {
        for (int i = startSearchIndex; i < tokens.Count; i++)
        {
            IToken currentToken = tokens.ElementAt(i);
            if (currentToken.Type == TokenType.Class)
            {
                return (currentToken, i);
            }
        }
        throw new Exception("class token not found");
    }

    public static (IToken? token, int findIndex) GetClassDependency(this IReadOnlyCollection<IToken> tokens, int startSearchIndex)
    {
        for (int i = startSearchIndex; i < tokens.Count; i++)
        {
            IToken currentToken = tokens.ElementAt(i);
            if (currentToken.Type == TokenType.Dependency)
            {
                return (currentToken, i);
            }
        }

        return (null, tokens.Count);
    }
    
    public static (IToken? token, int findIndex) GetClassRoute(this IReadOnlyCollection<IToken> tokens, int startSearchIndex)
    {
        for (int i = startSearchIndex; i < tokens.Count; i++)
        {
            IToken currentToken = tokens.ElementAt(i);
            if (currentToken.Type == TokenType.Route)
            {
                return (currentToken, i);
            }
            // it's mean class doesn't have route define
            if (currentToken.Type == TokenType.Endpoint)
            {
                return (null, tokens.Count);
            }
        }

        return (null, tokens.Count);
    }
}