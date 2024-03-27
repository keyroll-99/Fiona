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
using Microsoft.Extensions.Logging;

namespace Fiona.Hosting;

public sealed class FionaHostBuilder : IFionaHostBuilder
{
    private static IFionaHost? _host;
    private readonly AppConfig _appConfig;
    private readonly object _lock = new();
    private readonly RouterBuilder _routerBuilder = new();
    private readonly Assembly _startupAssembly;
    private Action<ILoggingBuilder> _loggerConfiguration = (b) => b.AddConsole();
        
    private FionaHostBuilder(Assembly assembly)
    {
        _startupAssembly = assembly;
        _appConfig = ConfigurationLoader.GetConfig(assembly);
        Configuration = new ConfigurationBuilder()
            .SetBasePath(Path.GetDirectoryName(assembly.Location))
            .AddJsonFile("AppSettings/AppSettings.json", false)
            .AddJsonFile($"AppSettings/AppSettings.{_appConfig.Environment}.json", true)
            .AddEnvironmentVariables()
            .Build();
    }

    public IConfiguration Configuration { get; }
    public ServiceCollection Service { get; } = new();

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

    public static IFionaHostBuilder CreateHostBuilder()
    {
        Assembly y = Assembly.GetCallingAssembly();
        return new FionaHostBuilder(Assembly.GetCallingAssembly());
    }

    private void CreateHost()
    {
        if (_host is not null) return;

        lock (_lock)
        {
            PrepareHostToRun();
            _host ??= new FionaHost(Service.BuildServiceProvider(), _appConfig);
        }
    }

    private void PrepareHostToRun()
    {
        Service.AddLogging(_loggerConfiguration);
        Service.AddSingleton<Router>(sp => _routerBuilder.Build(sp));
        AddMiddleware<CallEndpointMiddleware>();
        Service.AddTransient<MiddlewareCallStack>();
        Service.AddTransient<IConfiguration>(sp => Configuration);
        Service.AddSingleton<OptionFactory>();
    }

    private void LoadControllers()
    {
        foreach (Type controllerType in ControllerUtils.GetControllers(_startupAssembly))
        {
            Service.AddTransient(controllerType);
            _routerBuilder.AddController(controllerType);
        }
    }
    
    public void ConfigureLogging(Action<ILoggingBuilder> loggerConfiguration)
    {
        _loggerConfiguration = loggerConfiguration;
    }
}