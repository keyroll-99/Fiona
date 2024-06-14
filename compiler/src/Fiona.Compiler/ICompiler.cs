using Fiona.Compiler.Parser.Builders;

namespace Fiona.Compiler;

public interface ICompiler
{
    public Task RunAsync(IEnumerable<ProjectFile> projectFiles);
    public Task CompileFile(ProjectFile projectFile);
}