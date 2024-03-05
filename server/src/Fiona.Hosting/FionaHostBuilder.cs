using System.Reflection;
using Fiona.Hosting.Abstractions;
using Fiona.Hosting.Controller;
using Fiona.Hosting.Models;
using Fiona.Hosting.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Fiona.Hosting;

public sealed class FionaHostBuilder : IFionaHostBuilder
{
    public ServiceCollection Service { get; } = new();
    private static IFionaHost? _host = null;
    private readonly HostConfig _config = new();
    private readonly object _lock = new();
    private readonly Assembly _startupAssembly;
    private readonly RouterBuilder _routerBuilder = new();

    private FionaHostBuilder(Assembly assembly)
    {
        _startupAssembly = assembly;
    }

    public static IFionaHostBuilder CreateHostBuilder()
    {
        return new FionaHostBuilder(Assembly.GetCallingAssembly());
    }

    public IFionaHost Build()
    {
        LoadControllers();
        CreateHost();
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


    private void CreateHost()
    {
        if (_host is not null)
        {
            return;
        }

        lock (_lock)
        {
            Service.AddSingleton<Router>(sp => _routerBuilder.Build(sp));
            _host ??= new FionaHost(Service.BuildServiceProvider(), _config);
        }
    }

    private void LoadControllers()
    {
        foreach (var controllerType in ControllerUtils.GetControllers(_startupAssembly))
        {
            Service.AddTransient(controllerType);
            _routerBuilder.AddController(controllerType);
        }
    }
}