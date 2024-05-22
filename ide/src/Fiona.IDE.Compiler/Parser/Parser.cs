using Fiona.IDE.Compiler.Parser.Exceptions;
using Fiona.IDE.Compiler.Parser.Models;
using Fiona.IDE.Compiler.Tokens;
using Fiona.IDE.ProjectManager.Models;
using Microsoft.VisualBasic;
using System.Text;

namespace Fiona.IDE.Compiler.Parser;

internal sealed class Parser : IParser
{

    public async Task<ReadOnlyMemory<byte>> ParseAsync(IReadOnlyCollection<IToken> tokens, ProjectFile projectFile)
    {
        try
        {
            await Validator.ValidateAsync(tokens);
        }
        catch (ValidationError e)
        {
            throw new ParserException(projectFile.Name, e.Message);
        }

        StringBuilder stringBuilder = new StringBuilder(20_000);
        for (int i = 0; i < tokens.Count; i++)
        {
            IToken currentElement = tokens.ElementAt(i);
            switch (currentElement.Type)
            {
                case TokenType.UsingBegin:
                    i = AppendUsing(stringBuilder, tokens, i + 1);
                    stringBuilder.Append("\n namespace Token.Test\n");
                    continue;
                case TokenType.Class:
                    i = AppendClass(stringBuilder, tokens, i);
                    continue;
            }
        }
        var rest = stringBuilder.ToString();
        return new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes(rest));
    }


    private static int AppendUsing(StringBuilder stringBuilder, IReadOnlyCollection<IToken> tokens, int startIndex)
    {
        for (int i = startIndex; i < tokens.Count; i++)
        {
            IToken currentToken = tokens.ElementAt(i);
            if (currentToken.Type == TokenType.UsingEnd)
            {
                return i;
            }
            stringBuilder.Append($"using {currentToken.Value};\n");
        }
        // It check validator and it shouldn't be throw
        throw new ValidationError("Not found end of using statement.");
    }

    private static int AppendClass(StringBuilder stringBuilder, IReadOnlyCollection<IToken> tokens, int index)
    {
        IToken classToken = tokens.ElementAt(index++);
        IToken? controllerRoute = null;
        List<Dependency>? dependencies = null;
        List<Endpoint> endpoints = [];

        for (int i = index; i < tokens.Count; i++)
        {
            IToken currentToken = tokens.ElementAt(i);
            switch (currentToken.Type)
            {
                case TokenType.Route:
                    controllerRoute = currentToken;
                    continue;
                case TokenType.Dependency:
                    dependencies = Dependency.GetDependenciesFromToken(currentToken);
                    continue;
                case TokenType.Endpoint:
                    (Endpoint? endpoint, int endIndex) = GetNextEndpoint(tokens, i);
                    if (endpoint is null)
                    {
                        break;
                    }
                    endpoints.Add(endpoint);
                    i = endIndex;
                continue;
            }
        }
        
        Class @class = new(endpoints, controllerRoute?.Value, classToken.Value!, dependencies);
        stringBuilder.Append(@class.GenerateSourceCode());
        return tokens.Count;
    }

    // Maybe it should be move to facotry class?
    private static (Endpoint? endpoint, int endIndex) GetNextEndpoint(IReadOnlyCollection<IToken> tokens, int index)
    {

        IToken? routeToken = null;
        IToken? methodToken = null;
        IToken? returnType = null;
        List<Parameter> parameters = [];
        List<Dependency> dependencies = [];
        IToken? endpointToken = tokens.ElementAt(index++);

        for (int i = index; i < tokens.Count; i++)
        {
            IToken currentToken = tokens.ElementAt(i);
            switch (currentToken.Type)
            {
                case TokenType.Method:
                    methodToken = currentToken;
                    continue;
                case TokenType.Route:
                    routeToken = currentToken;
                    continue;
                case TokenType.ReturnType:
                    returnType = currentToken;
                    continue;
                case TokenType.Parameter:
                    parameters = Parameter.GetParametersFromToken(currentToken);
                    continue;
                case TokenType.Dependency:
                    dependencies = Dependency.GetDependenciesFromToken(currentToken);
                    continue;
                case TokenType.BodyBegin:
                    IReadOnlyCollection<IToken> bodyTokens = GetBodyTokens(tokens, i + 1);
                    Endpoint endpoint = new(endpointToken.Value!,
                                            routeToken?.Value,
                                            methodToken?.Value,
                                            returnType?.Value,
                                            parameters,
                                            bodyTokens);
                    return (endpoint, i + bodyTokens.Count + 1);
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }
        return (null, index);
    }

    private static IReadOnlyCollection<IToken> GetBodyTokens(IReadOnlyCollection<IToken> tokens, int startIndex)
    {
        List<IToken> result = [];
        for (int i = startIndex; i < tokens.Count; i++)
        {
            IToken currentToken = tokens.ElementAt(i);
            if (currentToken.Type == TokenType.BodyEnd)
            {
                break;
            }
            result.Add(currentToken);
        }

        return result;
    }
}