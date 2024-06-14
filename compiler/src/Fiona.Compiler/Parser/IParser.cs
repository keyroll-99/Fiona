using Fiona.Compiler.Parser.Builders;
using Fiona.Compiler.Tokenizer;

namespace Fiona.Compiler.Parser;

internal interface IParser
{
    public Task<ReadOnlyMemory<byte>> ParseAsync(IReadOnlyCollection<IToken> tokens, ProjectFile projectFile);// maybe it should be a stream or readOnlyMemory<byte>
}