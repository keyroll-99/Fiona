using Fiona.IDE.Compiler.Tokens;
using Fiona.IDE.ProjectManager;
using Fiona.IDE.ProjectManager.Models;

namespace Fiona.IDE.Compiler
{
    internal sealed class Compiler(IProjectManager projectManager, IEnumerable<IToken> tokens) : ICompiler
    {
        private readonly IProjectManager _projectManager = projectManager;
        private readonly IEnumerable<IToken> _tokens = tokens;

        public async Task RunAsync()
        {
            IEnumerable<ProjectFile> projectFiles = _projectManager.GetFiles();
            foreach (ProjectFile projectFile in projectFiles)
            {
                await using FileStream file = File.Open(projectFile.Path, FileMode.Open);
                await using FileCompiler fileCompiler = new FileCompiler(file, _tokens);
                await fileCompiler.CompileAsync();
            }
        }
        
    }
}