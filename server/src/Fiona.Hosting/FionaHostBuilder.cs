using System.Reflection;
using Fiona.Hosting.Abstractions;
using Fiona.Hosting.Abstractions.Configuration;
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
    private readonly AppConfig _appConfig;

    private FionaHostBuilder(Assembly assembly)
    {
        _startupAssembly = assembly;
        _appConfig = ConfigurationLoader.GetConfig(assembly);

        Configuration = new ConfigurationBuilder()
            .SetBasePath(Path.GetDirectoryName(assembly.Location))
            .AddJsonFile("AppSettings/AppSettings.json", optional: false)
            .AddJsonFile($"AppSettings/AppSettings.{_appConfig.Environment}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
    }

    public static IFionaHostBuilder CreateHostBuilder()
    {
        var y = Assembly.GetCallingAssembly();
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
            _host ??= new FionaHost(Service.BuildServiceProvider(), _appConfig);
        }
    }

    private void PrepareHostToRun()
    {
        Service.AddSingleton<Router>(sp => _routerBuilder.Build(sp));
        AddMiddleware<CallEndpointMiddleware>();
        Service.AddTransient<MiddlewareCallStack>();
        Service.AddTransient<IConfiguration>(sp => Configuration);
        Service.AddSingleton<OptionFactory>();
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
        Service.AddSingleton<IOption<T>>(sp => sp.GetRequiredService<OptionFactory>().CreateOption<T>(section));
        return this;
    }

    public IFionaHostBuilder AddConfig<T>() where T : class, new()
    {
        Service.AddSingleton<IOption<T>>(sp => sp.GetRequiredService<OptionFactory>().CreateOption<T>());
        T config = new();
        Configuration.GetSection(nameof(T)).Bind(config);
        return this;
    }
}