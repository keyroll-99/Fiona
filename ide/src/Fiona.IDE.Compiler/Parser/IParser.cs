using Fiona.IDE.Compiler.Tokens;
using Fiona.IDE.ProjectManager.Models;

namespace Fiona.IDE.Compiler.Parser;

internal interface IParser
{
    public Task<string> ParseAsync(IEnumerable<IToken> tokens, ProjectFile projectFile); // maybe it should be a stream or readOnlyMemory<byte>
}