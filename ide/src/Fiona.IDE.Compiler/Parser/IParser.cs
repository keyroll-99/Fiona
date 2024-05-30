using Fiona.IDE.ProjectManager.Models;
using Fiona.IDE.Tokenizer;

namespace Fiona.IDE.Compiler.Parser;

internal interface IParser
{
    public Task<ReadOnlyMemory<byte>> ParseAsync(IReadOnlyCollection<IToken> tokens, ProjectFile projectFile); // maybe it should be a stream or readOnlyMemory<byte>
}