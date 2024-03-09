using Fiona.Hosting.Controller;

namespace Fiona.Hosting.Tests.FionaServer.Controller;

[Controller]
public sealed class HomeController
{
    public Task<string> Index()
    {
        return Task.FromResult("Home");
    }
}