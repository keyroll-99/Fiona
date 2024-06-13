

using PowerArgs;

internal class RunCompilerArg
{
    [ArgRequired(PromptIfMissing = true), ArgDescription("Path to fsln file"), ArgPosition(1)]
    public required string Project { get; init; }
}