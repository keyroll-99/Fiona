using Fiona.Hosting.Abstractions;
using Fiona.Hosting.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Fiona.Hosting;

public sealed class FionaHostBuilder : IFionaHostBuilder
{
    public ServiceCollection Service { get; } = new();

    private static IFionaHost? _host = null;
    private readonly HostConfig _config = new();
    private readonly object _lock = new();
    
    public static IFionaHostBuilder CreateHost()
    {
        return new FionaHostBuilder();
    }

    public IFionaHost Build()
    {
        if (_host is null)
        {
            lock (_lock)
            {
                _host ??= new FionaHost(Service.BuildServiceProvider(), _config);
            }
        }

        return _host;
    }

    public IFionaHostBuilder SetUrl(string url)
    {
        _config.Url = url;
        return this;
    }

    public IFionaHostBuilder SetPort(string url)
    {
        _config.Port = url;
        return this;
    }
}