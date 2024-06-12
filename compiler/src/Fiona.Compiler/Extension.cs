using Fiona.Compiler.Parser;
using Microsoft.Extensions.DependencyInjection;

namespace Fiona.Compiler;

public static class Extension
{
    public static IServiceCollection AddCompiler(this IServiceCollection services)
    {
        services.AddSingleton<ICompiler, Compiler>();
        services.AddSingleton<IParser, Parser.Parser>();
        return services;
    }
}