using Fiona.Hosting.Abstractions.Middleware;
using Microsoft.Extensions.DependencyInjection;

namespace Fiona.Hosting.Abstractions;

public interface IFionaHostBuilder
{
    public ServiceCollection Service { get; }
    IFionaHost Build();
    IFionaHostBuilder AddMiddleware<T>() where T : IMiddleware;
}