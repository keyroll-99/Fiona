using Fiona.Hosting.Abstractions;
using Fiona.Hosting.TestServer.Models;

namespace Fiona.Hosting.TestServer;

public class FionaTestServerStartup : IDisposable
{
    private readonly Action<IFionaHostBuilder> _configure;

    public FionaTestServerStartup(Action<IFionaHostBuilder> configure)
    {
        _configure = configure;
        Builder = FionaHostBuilder.CreateHostBuilder();
    }

    public IFionaHostBuilder Builder { get; } = null!;
    private IFionaHost Host { get; set; } = null!;

    public void Dispose()
    {
        Host.Dispose();
    }

    public void Run(string port = "7000")
    {
        _configure(Builder);
        Builder.AddConfig<ConfigModel>();
        RunServer("7000");
    }

    private void RunServer(string port)
    {
        Host = Builder.Build();
        Task.Run(Host.Run);
    }
}