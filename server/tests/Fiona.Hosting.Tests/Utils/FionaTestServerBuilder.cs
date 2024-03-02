using Fiona.Hosting.Abstractions;

namespace Fiona.Hosting.Tests.Utils;

public class FionaTestServerBuilder : IDisposable
{
    public IFionaHost Host { get; private set; } = null!;
    public IFionaHostBuilder Builder { get; private set; } = null!;

    public FionaTestServerBuilder() 
    {
        RunServer("7000");
    }

    public void RunServer(string port)
    {
        Builder = FionaHostBuilder.CreateHostBuilder();
        Builder.SetPort(port);
        Host = Builder.Build();
        Task.Run(Host.Run);
    }

    public void Dispose()
    {
        Host.Dispose();
    }
}