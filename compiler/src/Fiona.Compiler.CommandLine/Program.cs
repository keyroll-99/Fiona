
using PowerArgs;

await Args.InvokeActionAsync<FionaCompilerProgram>(args);

[ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
internal class FionaCompilerProgram
{
    [ArgActionMethod, ArgDescription("Run compiler for program")]
    public async Task Compile(RunCompilerArg arg)
    {
        Console.WriteLine(arg.Project);
    }

    [ArgActionMethod]
    public async Task Test()
    {
        await Task.CompletedTask;
        Console.WriteLine("test");
    }
}
