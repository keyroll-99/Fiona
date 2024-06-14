using PowerArgs;

namespace Fiona.Compiler.CommandLine.Models;

internal class CreateSolutionArgs
{
    [ArgRequired(PromptIfMissing = true), ArgDescription("path to folder"), ArgPosition(1)]
    public required string Path { get; init; }
    
    [ArgRequired(PromptIfMissing = true), ArgDescription("name of solution"), ArgPosition(2)]
    public required string Name { get; init; }
}

internal class CompileFileArgs
{
    
    [ArgRequired(PromptIfMissing = true), ArgDescription("Patch to file"), ArgPosition(1)]
    public required string PathToFile { get; init; }
    
    [ArgDescription("Path to fsln file (required if file is in different folder)"), ArgPosition(2)]
    public string? PathToProject { get; init; }
}

internal class CreateFnFileArgs
{
    
}

internal class RunCompilerArg
{
    [ArgRequired(PromptIfMissing = true), ArgDescription("Path to fsln file"), ArgPosition(1)]
    public required string Project { get; init; }
}