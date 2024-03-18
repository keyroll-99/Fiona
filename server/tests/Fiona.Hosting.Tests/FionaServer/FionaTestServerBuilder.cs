using Fiona.Hosting.Abstractions;
using Fiona.Hosting.Tests.FionaServer.Mock;
using Fiona.Hosting.TestServer;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Fiona.Hosting.Tests.FionaServer;

public class FionaTestServerBuilder : IDisposable
{
    // public IFionaHost Host { get; private set; } = null!;
    // public IFionaHostBuilder Builder { get; private set; } = null!;
    public ICallMock CallMock { get; } = Substitute.For<ICallMock>();

    public FionaTestServerStartup FionaTestServerStartup;

    public FionaTestServerBuilder()
    {
        FionaTestServerStartup = new FionaTestServerStartup(builder =>
        {
            builder.Service.AddSingleton(CallMock);
            builder.AddMiddleware<CustomMiddleware>();
        });
        
        FionaTestServerStartup.Run();
    }
    
    public void Dispose()
    {
        FionaTestServerStartup.Dispose();
    }
}