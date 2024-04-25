using Fiona.IDE.ProjectManager;

namespace Fiona.IDE.Compiler
{
    internal sealed class Compiler(IProjectManager projectManager) : ICompiler
    {
        private readonly IProjectManager _projectManager = projectManager;

        public Task RunAsync()
        {
            
        }
    }
}