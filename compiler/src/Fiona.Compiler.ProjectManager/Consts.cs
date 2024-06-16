namespace Fiona.Compiler.ProjectManager;

public static class Constans
{
    public const string DefaultProgramCsValue = """
                                                using Fiona.Hosting;
                                                using Fiona.Hosting.Abstractions;

                                                IFionaHostBuilder serviceBuilder = FionaHostBuilder.CreateHostBuilder();
                                                using IFionaHost host = serviceBuilder.Build();
                                                host.Run();
                                                """;

    public const string DefaultServerSettings = """
                                         {
                                           "Port": "7000",
                                           "Environment": "Development"
                                         } 
                                         """;
}