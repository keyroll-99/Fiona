using Fiona.IDE.ProjectManager.Models;

namespace Fiona.IDE.Compiler
{
    public interface ICompiler
    {
        public Task RunAsync();
        public Task CompileFile(ProjectFile projectFile);
    }
}