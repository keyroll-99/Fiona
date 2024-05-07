using Fiona.IDE.Compiler.Tokens;
using Fiona.IDE.ProjectManager.Models;

namespace Fiona.IDE.Compiler.Parser;

internal sealed class Parser(Validator validator) : IParser
{

    public async Task<string> ParseAsync(IEnumerable<IToken> tokens, ProjectFile projectFile)
    {
        if (!await validator.ValidateAsync(tokens))
        {
            throw new Exception();
        }
        return "parsed";
    }
}