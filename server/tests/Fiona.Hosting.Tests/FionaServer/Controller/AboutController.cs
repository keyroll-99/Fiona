using Fiona.Hosting.Controller;
using Fiona.Hosting.Routing;

namespace Fiona.Hosting.Tests.FionaServer.Controller;

[Controller("about")]
public sealed class AboutController
{
    public Task<string> Index()
    {
        return Task.FromResult("About");
    }
    
    [Route(HttpMethodType.Get | HttpMethodType.Post, "another/route")]
    public Task<string> AnotherRoute()
    {
        return Task.FromResult("another/route");
    }
}