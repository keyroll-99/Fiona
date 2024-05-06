using Fiona.IDE.Compiler.Tokens;
using Fiona.IDE.ProjectManager.Models;

namespace Fiona.IDE.Compiler.Parser;

internal class Parser : IParser
{



    public Task ParseAsync(IReadOnlyCollection<IToken> tokens, ProjectFile projectFile)
    {
        return Task.CompletedTask;
    }
}