namespace FionaServerTest.Utils;

public class FionaTestServerBuilder(string port = "7000") : IDisposable
{
    private FionaServer.FionaServer _fionaServer = new(port);
    private Task _serverTask;

    public void RunServer()
    {
        _serverTask = Task.Run(() => _fionaServer.Start());
    }

    public void Dispose()
    {
        _fionaServer.Dispose();
        _serverTask.Wait();
    }
}