using Fiona.Hosting.Controller.Attributes;
using Fiona.Hosting.Routing;
using Fiona.Hosting.Routing.Attributes;

namespace Fiona.Hosting.TestServer.Controller;

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