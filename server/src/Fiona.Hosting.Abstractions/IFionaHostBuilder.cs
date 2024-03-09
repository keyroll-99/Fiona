using Microsoft.Extensions.DependencyInjection;

namespace Fiona.Hosting.Abstractions;

public interface IFionaHostBuilder
{
    public ServiceCollection Service { get; }
    IFionaHost Build();
    IFionaHostBuilder SetUrl(string url);
    IFionaHostBuilder SetPort(string url);
    IFionaHostBuilder AddMiddleware<T>() where T : IMiddleware;
}