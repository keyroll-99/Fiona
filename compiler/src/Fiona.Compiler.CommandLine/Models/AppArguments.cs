using PowerArgs;

namespace Fiona.Compiler.CommandLine.Models;

internal class CreateSolutionArgs
{
    [ArgRequired(PromptIfMissing = true), ArgDescription("path to folder"), ArgExistingDirectory, ArgPosition(1)]
    public required string Path { get; init; }

    [ArgRequired(PromptIfMissing = true), ArgDescription("name of solution"), ArgPosition(2)]
    public required string Name { get; init; }
}

internal class CompileFileArgs
{

    [ArgRequired(PromptIfMissing = true), ArgDescription("Patch to file"), ArgExistingDirectory, ArgPosition(1)]
    public required string PathToFile { get; init; }

    [ArgDescription("Path to folder with fsln file"), ArgExistingDirectory, ArgPosition(2)]
    public string? Project { get; set; }
}

internal class CreateFnFileArgs
{
    [ArgDescription("Name of file"), ArgPosition(1)]
    public required string Name { get; init; }

    [ArgDescription("Path to file location"), ArgExistingDirectory, ArgPosition(2)]
    public string? Path { get; set; }

    [ArgDescription("Path to fsln file (required if file is in different folder)"), ArgExistingDirectory, ArgPosition(3)]
    public string? PathToProject { get; set; }
}

internal class RunCompilerArg
{
    [ArgDescription("Path to folder with fsln file"), ArgExistingDirectory, ArgPosition(1)]
    public string? Project { get; set; }
}