using Microsoft.Extensions.DependencyInjection;

namespace Fiona.IDE.Compiler
{
    public static class Extension
    {
        public static IServiceCollection AddCompiler(this IServiceCollection services)
        {
            services.AddSingleton<ICompiler, Compiler>();
            return services;
        }
    }
}