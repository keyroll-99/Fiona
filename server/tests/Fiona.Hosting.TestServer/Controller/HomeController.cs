using Fiona.Hosting.Controller.Attributes;
using Microsoft.Extensions.Logging;

namespace Fiona.Hosting.TestServer.Controller;

[Controller]
public sealed class HomeController(ILogger logger)
{
    private readonly ILogger _logger = logger;

    public Task<string> Index()
    {
        _logger.Log(LogLevel.Critical, "Home Controller Index");
        return Task.FromResult("Home");
    }
}