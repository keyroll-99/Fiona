namespace Fiona.IDE.Compiler.Parser.Exceptions;

public sealed class ParserException(string fileName) : Exception($"Cannot parse the file. {fileName}")
{
    
}