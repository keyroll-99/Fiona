namespace Fiona.Compiler;

public static class CompilerFactory
{
    public static ICompiler Create() => new Compiler(new Parser.Parser());
}