using Fiona.Hosting.Controller.Attributes;
using Microsoft.Extensions.Logging;

namespace SampleFionaServer.Controller;

[Controller("home")]
public class HomeController(ILogger<HomeController> logger)
{
    private readonly ILogger<HomeController> _logger = logger;
    
    public Task<string> Index()
    {
        _logger.Log(LogLevel.Critical, "Home Controller Index");
        return Task.FromResult("Home");
    }
}