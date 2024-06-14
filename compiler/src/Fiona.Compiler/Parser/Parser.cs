using Fiona.Compiler.Parser.Builders;
using Fiona.Compiler.Parser.Exceptions;
using Fiona.Compiler.Tokenizer;
using System.Text;

namespace Fiona.Compiler.Parser;

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
        StringBuilder stringBuilder = new(20_000);
        for (int i = 0; i < tokens.Count; i++)
        {
            IToken currentElement = tokens.ElementAt(i);
            switch (currentElement.Type)
            {
                case TokenType.UsingBegin:
                    i = AppendUsing(stringBuilder, tokens, i + 1);
                    continue;
                case TokenType.Namespace:
                    stringBuilder.AppendFormat("namespace {0};\n", currentElement.Value);
                    continue;
                case TokenType.Class:
                    i = AppendClass(stringBuilder, tokens, i);
                    continue;
            }
        }
        string rest = stringBuilder.ToString();
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
        List<DependencyBuilder>? dependencies = null;
        List<EndpointBuilder> endpoints = [];

        for (int i = index; i < tokens.Count; i++)
        {
            IToken currentToken = tokens.ElementAt(i);
            switch (currentToken.Type)
            {
                case TokenType.Route:
                    controllerRoute = currentToken;
                    continue;
                case TokenType.Dependency:
                    dependencies = DependencyBuilder.GetDependenciesFromToken(currentToken);
                    continue;
                case TokenType.Endpoint:
                    (EndpointBuilder? endpoint, int endIndex) = GetNextEndpoint(tokens, i);
                    if (endpoint is null)
                    {
                        break;
                    }
                    endpoints.Add(endpoint);
                    i = endIndex;
                    continue;
            }
        }

        ClassBuilder classBuilder = new(endpoints, controllerRoute?.Value, classToken.Value!, dependencies);
        stringBuilder.Append(classBuilder.GenerateSourceCode());
        return tokens.Count;
    }

    // Maybe it should be move to facotry class?
    private static (EndpointBuilder? endpoint, int endIndex) GetNextEndpoint(IReadOnlyCollection<IToken> tokens, int index)
    {

        IToken? routeToken = null;
        IToken? methodToken = null;
        IToken? returnType = null;
        List<ParameterBuilder> parameters = [];
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
                    parameters = ParameterBuilder.GetParametersFromToken(currentToken);
                    continue;
                case TokenType.BodyBegin:
                    IToken? bodyToken = GetBodyToken(tokens, i + 1);
                    EndpointBuilder endpointBuilder = new(endpointToken.Value!,
                                            routeToken?.Value,
                                            methodToken?.Value,
                                            returnType?.Value,
                                            parameters,
                                            bodyToken);
                    return (endpointBuilder, bodyToken is not null ? i + 2 : i + 1);
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }
        return (null, index);
    }

    private static IToken? GetBodyToken(IReadOnlyCollection<IToken> tokens, int startIndex) => tokens.ElementAt(startIndex).Type == TokenType.Body ? tokens.ElementAt(startIndex) : null;
}