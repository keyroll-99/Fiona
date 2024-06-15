using Fiona.Compiler;
using Fiona.Compiler.CommandLine.Models;
using Fiona.Compiler.ProjectManager;
using PowerArgs;
using ProjectFile=Fiona.Compiler.ProjectManager.Models.ProjectFile;


List<string> dummyArgs = new()
{
    "CreateEndpoint", "TestFile3", "E:\\100comitow\\ConsoleApp\\FnFiles" , "E:\\100comitow\\ConsoleApp"
};

List<string> dummyArgs2 = new()
{
    "Create", "E:\\100comitow\\ConsoleApp", "TestConsole"
};

//CompileFile  E:\100comitow\ConsoleApp\TestFromConsole\aaa.fn E:\100comitow\ConsoleApp
await Args.InvokeActionAsync<FionaCompilerProgram>(dummyArgs.ToArray());

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
        IProjectManager projectManager = await GetProjectManagerForFile(args.PathToFile, args.PathToProject);
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
        if (args.Path is null && args.PathToProject is null)
        {
            throw new Exception("you have to pass one of args: Path, PathToProject");
        }
        IProjectManager projectManager;
        if (args.Path is null)
        {
            projectManager = await ProjectManagerFactory.GetInstance(args.PathToProject!);
            args.Path = $"{args.PathToProject}{Path.DirectorySeparatorChar}{projectManager.GetName()}";
        }
        else
        {
            projectManager = await GetProjectManagerForFile(args.Path!, args.PathToProject);
        }

        await projectManager.CreateFileAsync(args.Name, args.Path!);
    }

    private static async Task<IProjectManager> GetProjectManagerForFile(string pathToFile, string? pathToFslnFile)
    {
        string? pathToProject = pathToFslnFile;
        if (string.IsNullOrWhiteSpace(pathToFslnFile))
        {
            pathToProject = Path.GetDirectoryName(pathToFile);
        }
        IProjectManager projectManager = await ProjectManagerFactory.GetInstance(pathToProject!);
        return projectManager;
    }

    [ArgActionMethod]
    public async Task Test()
    {
        await Task.CompletedTask;
        Console.WriteLine("test");
    }
}