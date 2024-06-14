using PowerArgs;

namespace Fiona.Compiler.CommandLine.Models;

internal class CreateSolutionArgs
{
    [ArgRequired(PromptIfMissing = true), ArgDescription("path to folder"), ArgPosition(1)]
    public required string Path { get; init; }
    
    [ArgRequired(PromptIfMissing = true), ArgDescription("name of solution"), ArgPosition(2)]
    public required string Name { get; init; }
}

internal class CompileFileArg
{
    [ArgRequired(PromptIfMissing = true), ArgDescription("Patch to file"), ArgPosition(1)]
    public required string PathToFile { get; init; }
}

internal class RunCompilerArg
{
    [ArgRequired(PromptIfMissing = true), ArgDescription("Path to fsln file"), ArgPosition(1)]
    public required string Project { get; init; }
}