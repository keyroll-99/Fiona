using Fiona.Hosting.Controller;

namespace Fiona.Hosting.Tests.Utils.Controller;

[Controller]
public class HomeController
{
    public Task<string> Index()
    {
        return Task.FromResult("Home");
    }
}