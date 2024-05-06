using Fiona.IDE.Compiler.Tokens;
using Fiona.IDE.ProjectManager.Models;

namespace Fiona.IDE.Compiler.Parser;

internal interface IParser
{
    public Task ParseAsync(IReadOnlyCollection<IToken> tokens, ProjectFile projectFile);
}