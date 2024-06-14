using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace Fiona.Compiler.ProjectManager;

public static class Extension
{
    public static IServiceCollection AddProjectManager(this IServiceCollection services)
    {
        services.AddSerilog((config) => config.WriteTo.Console(LogEventLevel.Information));
        services.AddScoped<ICommandRunner, CommandRunner>();
        services.AddSingleton<IProjectManager, Compiler.ProjectManager.ProjectManager>();
        return services;
    }
}