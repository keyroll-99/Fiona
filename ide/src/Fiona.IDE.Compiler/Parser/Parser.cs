using Fiona.IDE.Compiler.Parser.Exceptions;
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
        for(int i = 0; i < tokens.Count; i++)
        {
            IToken currentElement = tokens.ElementAt(i);
            switch (currentElement.Type)
            {
                case TokenType.UsingBegin:
                    i = AppendUsing(stringBuilder, tokens, i + 1);
                    continue;
            }
        }
        return new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes(stringBuilder.ToString()));
    }


    private static int AppendUsing(StringBuilder stringBuilder, IReadOnlyCollection<IToken> tokens, int startIndex)
    {
        for (int i = startIndex; i < tokens.Count; i++)
        {
            // if(s)
            // stringBuilder.Append("")
        }
    }
}