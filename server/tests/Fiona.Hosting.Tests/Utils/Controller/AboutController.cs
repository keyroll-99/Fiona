using Fiona.Hosting.Controller;
using Fiona.Hosting.Routing;

namespace Fiona.Hosting.Tests.Utils.Controller;

[Controller("about")]
public class AboutController
{
    public Task<string> Index()
    {
        return Task.FromResult("Home");
    }
    
    [Route(HttpMethodType.Get | HttpMethodType.Post, "Dupa")]
    public Task<string> DupaResult()
    {
        return Task.FromResult("Home");
    }
}