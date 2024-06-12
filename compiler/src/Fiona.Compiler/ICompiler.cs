using Fiona.Compiler.Parser.Models;

namespace Fiona.Compiler;

public interface ICompiler
{
    public Task RunAsync(IEnumerable<ProjectFile> projectFiles);
    public Task CompileFile(ProjectFile projectFile);
}