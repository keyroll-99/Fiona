using Fiona.IDE.Compiler.Tokens;

namespace Fiona.IDE.Compiler;

internal sealed class FileCompiler(FileStream file, IEnumerable<IToken> tokens) : IDisposable, IAsyncDisposable
{
    private readonly IEnumerable<IToken> _tokens = tokens;
    
    public Task CompileAsync()
    {
        return Task.CompletedTask;
    }
    
    public void Dispose()
    {
        file.Dispose();
    }
    public async ValueTask DisposeAsync()
    {
        await file.DisposeAsync();
    }
}