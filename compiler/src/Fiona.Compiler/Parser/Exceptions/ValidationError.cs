namespace Fiona.Compiler.Parser.Exceptions;

internal sealed class ValidationError(string message) : Exception(message)
{
}