using Fiona.Hosting.Tests.FionaServer.Mock;
using Fiona.Hosting.TestServer;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Fiona.Hosting.Tests.FionaServer;

public class FionaTestServerBuilder : IDisposable
{
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

    public ICallMock CallMock { get; } = Substitute.For<ICallMock>();

    public void Dispose()
    {
        FionaTestServerStartup.Dispose();
    }
}