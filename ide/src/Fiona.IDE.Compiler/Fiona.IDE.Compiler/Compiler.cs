namespace Fiona.IDE.Compiler
{
    internal sealed class Compiler : ICompiler
    {
        public Task Run()
        {
            return Task.CompletedTask;
        }
    }
}