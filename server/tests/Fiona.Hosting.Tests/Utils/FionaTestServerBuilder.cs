using Fiona.Hosting.Abstractions;
using Fiona.Hosting.Tests.Utils.Mock;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Fiona.Hosting.Tests.Utils;

public class FionaTestServerBuilder : IDisposable
{
    public IFionaHost Host { get; private set; } = null!;
    public IFionaHostBuilder Builder { get; private set; } = null!;

    public ICallMock CallMock { get; } = Substitute.For<ICallMock>();

    public FionaTestServerBuilder() 
    {
        RunServer("7000");
    }

    private void RunServer(string port)
    {
        Builder = FionaHostBuilder.CreateHostBuilder();
        Builder.SetPort(port);
        Builder.Service.AddSingleton<ICallMock>(CallMock);
        Builder.AddMiddleware<CustomMiddleware>();
        
        Host = Builder.Build();
        Task.Run(Host.Run);
    }

    public void Dispose()
    {
        Host.Dispose();
    }
}