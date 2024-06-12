namespace Fiona.Compiler.Parser.Exceptions;

public sealed class ParserException(string fileName, string errorMessage) : Exception($"Cannot parse the file. {fileName}, Error message: {errorMessage}")
{
}