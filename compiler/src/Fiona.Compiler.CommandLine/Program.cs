using Fiona.Compiler.CommandLine.Models;
using Fiona.Compiler.ProjectManager;
using PowerArgs;


await Args.InvokeActionAsync<FionaCompilerProgram>(args);

[ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
internal class FionaCompilerProgram
{
    [ArgActionMethod, ArgDescription("Run compiler for program")]
    public async Task Create(CreateSolutionArgs arg)
    {
        IProjectManager projectManager = ProjectManagerFactory.GetOrCreate();
        await projectManager.CreateProject(arg.Path, arg.Name);
    }

    [ArgActionMethod]
    public async Task Test()
    {
        await Task.CompletedTask;
        Console.WriteLine("test");
    }
}
