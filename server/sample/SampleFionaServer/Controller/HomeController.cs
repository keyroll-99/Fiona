using Fiona.Hosting.Controller.Attributes;
using Fiona.Hosting.Routing;
using Fiona.Hosting.Routing.Attributes;
using Microsoft.Extensions.Logging;

namespace SampleFionaServer.Controller;

[Controller]
public class HomeController(ILogger<HomeController> logger)
{
    private readonly ILogger<HomeController> _logger = logger;
    
    [Route(HttpMethodType.Get)]
    public Task<string> Index()
    {
        _logger.Log(LogLevel.Critical, "Home Controller Index");
        return Task.FromResult("Home");
    }
}