using System.Reflection;
using Fiona.Hosting.Abstractions;
using Fiona.Hosting.Abstractions.Middleware;
using Fiona.Hosting.Configuration;
using Fiona.Hosting.Controller;
using Fiona.Hosting.Middleware;
using Fiona.Hosting.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fiona.Hosting;

public sealed class FionaHostBuilder : IFionaHostBuilder
{
    public ServiceCollection Service { get; } = new();
    public IConfiguration Configuration { get; }
    private static IFionaHost? _host = null;
    private readonly object _lock = new();
    private readonly Assembly _startupAssembly;
    private readonly RouterBuilder _routerBuilder = new();

    private FionaHostBuilder(Assembly assembly)
    {
        _startupAssembly = assembly;
        Configuration = new ConfigurationBuilder().SetBasePath(Path.GetDirectoryName(assembly.Location)).Build();// TODO: add json files and environment
    }

    public static IFionaHostBuilder CreateHostBuilder()
    {
        return new FionaHostBuilder(Assembly.GetCallingAssembly());
    }

    public IFionaHost Build()
    {
        LoadControllers();
        CreateHost();
        return _host!;
    }

    public IFionaHostBuilder AddMiddleware<T>() where T : IMiddleware
    {
        Service.AddTransient(typeof(IMiddleware), typeof(T));
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
            PrepareHostToRun();
            _host ??= new FionaHost(Service.BuildServiceProvider(), ConfigurationLoader.GetConfig(_startupAssembly));
        }
    }

    private void PrepareHostToRun()
    {
        Service.AddSingleton<Router>(sp => _routerBuilder.Build(sp));
        AddMiddleware<CallEndpointMiddleware>();
        Service.AddTransient<MiddlewareCallStack>();
        Service.AddTransient<IConfiguration>(sp => Configuration);
    }

    private void LoadControllers()
    {
        foreach (var controllerType in ControllerUtils.GetControllers(_startupAssembly))
        {
            Service.AddTransient(controllerType);
            _routerBuilder.AddController(controllerType);
        }
    }

    public IFionaHostBuilder AddConfig<T>(string section) where T : class, new()
    {
        T config = new();
        Configuration.GetSection(section).Bind(config);
        Service.AddSingleton(config);
        return this;
    }
}