using Fiona.Compiler;
using Fiona.Compiler.CommandLine.Models;
using Fiona.Compiler.ProjectManager;
using PowerArgs;
using ProjectFile=Fiona.Compiler.ProjectManager.Models.ProjectFile;


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

    [ArgActionMethod, ArgDescription("Compile one file to c#")]
    public async Task CompileFile(CompileFileArgs args)
    {
        IProjectManager projectManager = ProjectManagerFactory.GetOrCreate();
        string? pathToProject = args.PathToProject;
        if (string.IsNullOrWhiteSpace(args.PathToProject))
        {
            pathToProject = Path.GetDirectoryName(args.PathToFile);
        }

        await projectManager.LoadProject(pathToProject);
        ProjectFile? projectFile = projectManager.GetFiles().FirstOrDefault(f => f.Path == args.PathToFile);
        if (projectFile is null)
        {
            throw new Exception($"project file now found");
        }

        ICompiler compiler = CompilerFactory.Create();
        await compiler.CompileFile(new Fiona.Compiler.Parser.Builders.ProjectFile(projectFile.Name, projectFile.Path));
    }

    [ArgActionMethod, ArgDescription("Create new fn file")]
    public async Task CreateEndpoint()
    {
        await Task.CompletedTask;
    }

    [ArgActionMethod]
    public async Task Test()
    {
        await Task.CompletedTask;
        Console.WriteLine("test");
    }
}