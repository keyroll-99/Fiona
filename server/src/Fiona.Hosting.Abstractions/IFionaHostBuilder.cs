using Fiona.Hosting.Abstractions.Middleware;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fiona.Hosting.Abstractions;

public interface IFionaHostBuilder
{
    public ServiceCollection Service { get; }
    IFionaHost Build();
    IFionaHostBuilder AddMiddleware<T>() where T : IMiddleware;
    IFionaHostBuilder AddConfig<T>(string section) where T : class, new();
    IFionaHostBuilder AddConfig<T>() where T : class, new();
    IFionaHostBuilder ConfigureLogging(Action<ILoggingBuilder> loggerConfiguration);
}