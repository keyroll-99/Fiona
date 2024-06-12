using Fiona.Compiler.Tokenizer;

namespace Fiona.IDE.ProjectManager.Models;

public sealed class Endpoint
{
    public string Name { get; }
    public string? Route { get; }
    public List<string> Methods { get; }
    public string ReturnType { get; }
    public List<Input> Inputs { get; }
    public string Body { get; }

    private Endpoint(string name, string? route, List<string> methods, string returnType, List<Input> inputs, string body)
    {
        Route = route;
        Methods = methods;
        ReturnType = returnType;
        Inputs = inputs;
        Body = body;
        Name = name;
    }

    public static (Endpoint? endpoint, int endOfSearch) GetNextEndpoint(IReadOnlyCollection<IToken> tokens, int startIndex)
    {
        string name = string.Empty;
        string route = string.Empty;
        List<string> methods = [];
        string returnType = string.Empty;
        List<Input> inputs = [];
        string body = string.Empty;
        
        for (int i = startIndex; i < tokens.Count; i++)
        {
            IToken currentToken = tokens.ElementAt(i);
            switch (currentToken.Type)
            {
                case TokenType.Endpoint:
                    name = currentToken.Value!;
                    continue;
                case TokenType.Route:
                    route = currentToken.Value!;
                    continue;
                case TokenType.Method:
                    methods = currentToken.Value!.Replace("[", "").Replace("]", "").Split(",").ToList();
                    continue;
                case TokenType.Body:
                    body = currentToken.Value!;
                    continue;
                case TokenType.Parameter:
                    inputs = Input.GetInputsFromToken(currentToken);
                    continue;
                case TokenType.BodyEnd:
                    return (new Endpoint(name, route, methods, returnType, inputs, body), i);
                case TokenType.ReturnType:
                    returnType = currentToken.Value!;
                    continue;
            }
        }

        return (null, tokens.Count);
    }
}