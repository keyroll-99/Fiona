using Fiona.Hosting.Abstractions;

namespace Fiona.Hosting.Tests.Utils;

public class FionaTestServerBuilder : IDisposable
{
    private IFionaHost _host;
    private Task _hostTask;
    

    public FionaTestServerBuilder(string port = "7000") 
    {
        RunServer(port);
    }

    public void RunServer(string port)
    {
        var builder = FionaHostBuilder.CreateHost();
        builder.SetPort(port);
        _host = builder.Build();
        _hostTask = Task.Run(() => _host.Run());
    }

    public void Dispose()
    {
        _host.Dispose();
    }
}