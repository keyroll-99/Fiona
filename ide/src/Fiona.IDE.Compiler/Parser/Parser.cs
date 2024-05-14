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
        return new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes(stringBuilder.ToString()));
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
        if(classToken.Type == TokenType.Route)
        {
            controllerRoute = tokens.ElementAt(index);
            index += 1;
        }
        List<Endpoint> endpoints = [];
        for (int i = index; i < tokens.Count; i++)
        {
            (Endpoint? endpoint, int endIndex) = GetNextEndpoint(tokens, i);
            if (endpoint is null)
            {
                break;
            }
            endpoints.Add(endpoint);
            i = endIndex;
        }       
 
        return tokens.Count;
    }
    
    
    private static (Endpoint? endpoint, int endIndex) GetNextEndpoint(IReadOnlyCollection<IToken> tokens, int index)
    {
        IToken? routeToken = null;
        IToken? methodToken = null;
        
        for(int i = index; i < tokens.Count; i++)
        {
            IToken currentToken = tokens.ElementAt(i);
            switch (cu)
            {
                
            }
            
        }
        return (null, index);
    }
}