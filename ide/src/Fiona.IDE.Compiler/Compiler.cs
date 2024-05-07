using Fiona.IDE.Compiler.Parser;
using Fiona.IDE.Compiler.Tokens;
using Fiona.IDE.ProjectManager;
using Fiona.IDE.ProjectManager.Models;
using System.Text;

namespace Fiona.IDE.Compiler
{
    internal sealed class Compiler(IProjectManager projectManager, IParser parser) : ICompiler
    {

        public async Task RunAsync()
        {
            IEnumerable<ProjectFile> projectFiles = projectManager.GetFiles();
            List<Task> parsingTask = projectFiles.Select(ParseFileAsync).ToList();
            await Task.WhenAll(parsingTask);
        }


        private async Task ParseFileAsync(ProjectFile projectFile)
        {
            
            await using FileStream file = File.Open(projectFile.Path, FileMode.Open);
            try
            {
                using StreamReader reader = new(file);
                IReadOnlyCollection<IToken> tokens = await Tokenizer.GetTokensAsync(reader);
                string fileContent = (await parser.ParseAsync(tokens, projectFile));
                ReadOnlyMemory<byte> memory = new(Encoding.UTF8.GetBytes(fileContent));
                await file.WriteAsync(memory);
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                file.Close();
            }
        }
        
    }
}