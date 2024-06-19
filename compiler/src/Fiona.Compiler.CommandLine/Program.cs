using Fiona.Compiler;
using Fiona.Compiler.CommandLine.Models;
using Fiona.Compiler.ProjectManager;
using PowerArgs;
using ProjectFile=Fiona.Compiler.ProjectManager.Models.ProjectFile;


List<string> createEndpointArgs = new()
{
    "CreateEndpoint", "TestController", "-Pr", "E:\\Test100comitow\\ConsoleApp"
};

List<string> createProjectArgs = new()
{
    "Create", "E:\\Test100comitow\\ConsoleApp", "TestConsole"
};

List<string> compileSolutionArgs = new()
{
    "Compile", "E:\\Test100comitow\\ConsoleApp"
};


List<string> runSolutionArgs = new()
{
    "Run", "E:\\Test100comitow\\ConsoleApp"
};

//CompileFile  E:\100comitow\ConsoleApp\TestFromConsole\aaa.fn E:\100comitow\ConsoleApp
await Args.InvokeActionAsync<FionaCompilerProgram>(compileSolutionArgs.ToArray());

[ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
internal class FionaCompilerProgram
{
    [ArgActionMethod, ArgDescription("Run compiler for program")]
    public async Task Create(CreateSolutionArgs arg)
    {
        IProjectManager projectManager = ProjectManagerFactory.GetInstance();
        await projectManager.CreateProject(arg.Path, arg.Name);
    }

    [ArgActionMethod, ArgDescription("Compile one file to c#")]
    public async Task CompileFile(CompileFileArgs args)
    {
        IProjectManager projectManager = await GetProject(args.Project);
        ProjectFile? projectFile = projectManager.GetFiles().FirstOrDefault(f => f.Path == args.PathToFile);
        if (projectFile is null)
        {
            throw new Exception($"project file now found");
        }
        ICompiler compiler = CompilerFactory.Create();
        await compiler.CompileFile(new Fiona.Compiler.Parser.Builders.ProjectFile(projectFile.Name, projectFile.Path));
    }

    [ArgActionMethod, ArgDescription("Create new fn file")]
    public async Task CreateEndpoint(CreateFnFileArgs args)
    {
        IProjectManager projectManager = await GetProject(args.Project);;
        args.Path ??= $"{args.Project}{Path.DirectorySeparatorChar}{projectManager.GetName()}";

        await projectManager.CreateFileAsync(args.Name, args.Path!);
    }

    [ArgActionMethod, ArgDescription("Compile all fs files to c#")]
    public async Task Compile(RunCompilerArg arg)
    {
        IProjectManager projectManager = await GetProject(arg.Project);
        ICompiler compiler = CompilerFactory.Create();
        await compiler.RunAsync(projectManager.GetFiles().Select(x => new Fiona.Compiler.Parser.Builders.ProjectFile(x.Name, x.Path)));
    }

    [ArgActionMethod, ArgDescription("Run project")]
    public async Task Run(RunProjectArg arg)
    {
        IProjectManager projectManager = await GetProject(arg.Project);
        ICompiler compiler = CompilerFactory.Create();
        await compiler.RunAsync(projectManager.GetFiles().Select(x => new Fiona.Compiler.Parser.Builders.ProjectFile(x.Name, x.Path)));
        await projectManager.RunAsync();
    }

    private static async Task<IProjectManager> GetProject(string? pathToFslnFile)
    {
        string pathToProject = pathToFslnFile ?? Environment.CurrentDirectory;
        IProjectManager projectManager = await ProjectManagerFactory.GetInstance(pathToProject);
        return projectManager;
    }

}