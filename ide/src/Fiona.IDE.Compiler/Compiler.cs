using Fiona.IDE.ProjectManager;
using Fiona.IDE.ProjectManager.Models;

namespace Fiona.IDE.Compiler
{
    internal sealed class Compiler(IProjectManager projectManager) : ICompiler
    {
        private readonly IProjectManager _projectManager = projectManager;

        public Task RunAsync()
        {
            IEnumerable<ProjectFile> projectFiles = _projectManager.GetFiles();
            foreach (ProjectFile projectFile in projectFiles)
            {
                // Compile projectFile
            }
            return Task.CompletedTask;
        }
        
    }
}