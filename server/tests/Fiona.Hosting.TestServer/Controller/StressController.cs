using Fiona.Hosting.Controller;
using Fiona.Hosting.Controller.Attributes;

namespace Fiona.Hosting.TestServer.Controller;

[Controller("stress")]
public class StressController
{
    public Task<string> Index()
    {
        Task.Delay(TimeSpan.FromMinutes(1));
        return Task.FromResult("Stress");
    }
}