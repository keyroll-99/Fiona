using Microsoft.Extensions.DependencyInjection;

namespace Fiona.IDE.ProjectManager
{
    public static class Extension
    {
        public static IServiceCollection AddProjectManager(this IServiceCollection services)
        {
            services.AddSingleton<IProjectManager, ProjectManager>();
            return services;
        }
    }
}