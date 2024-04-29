using Fiona.IDE.Compiler.Tokens;
using Fiona.IDE.ProjectManager;
using Fiona.IDE.ProjectManager.Models;

namespace Fiona.IDE.Compiler
{
    internal sealed class Compiler(IProjectManager projectManager) : ICompiler
    {

        public async Task RunAsync()
        {
            IEnumerable<ProjectFile> projectFiles = projectManager.GetFiles();
            foreach (ProjectFile projectFile in projectFiles)
            {
                await using FileStream file = File.Open(projectFile.Path, FileMode.Open);
            }
        }
        
    }
}