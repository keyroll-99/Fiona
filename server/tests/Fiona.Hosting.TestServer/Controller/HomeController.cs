using Fiona.Hosting.Controller;

namespace Fiona.Hosting.TestServer.Controller;

[Controller]
public sealed class HomeController
{
    public Task<string> Index()
    {
        return Task.FromResult("Home");
    }
}