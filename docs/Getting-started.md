# Getting started

## Download Fiona

## How to build a simple server from code.

1. In the beginning, you have to create the folder `AppSettings` and in the folder, you have to create two files

   1. `AppSettings.json`
   1. `ServerSetting.json`

1. in `Program.cs`

```c#
using Fiona.Hosting;
using Fiona.Hosting.Abstractions;

IFionaHostBuilder serviceBuilder = FionaHostBuilder.CreateHostBuilder();

using IFionaHost host = serviceBuilder.Build();

host.Run();
```

1. create sample controller

```c#
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
```

## Routing

```c#
using Fiona.Hosting.Controller.Attributes;
using Fiona.Hosting.Routing;
using Fiona.Hosting.Routing.Attributes;
using Microsoft.Extensions.Logging;

namespace SampleFionaServer.Controller;

[Controller]
public class HomeController(ILogger<HomeController> logger)
{
    private readonly ILogger<HomeController> _logger = logger;

    [Route(HttpMethodType.Get | HttpMethodType.Post)]
    public Task<string> Index()
    {
        _logger.Log(LogLevel.Critical, "Home Controller Index");
        return Task.FromResult("Home");
    }


    [Route(HttpMethodType.Delete, "{userId}")]
    public Task<string> DeleteUser(int userId)
    {
        _logger.Log(LogLevel.Critical, "Home Controller Index");
        return Task.FromResult("Home");
    }
}
```

[Read more about routing](Routing.md)

## How to inject own service

in `Program.cs`

```c#
using Fiona.Hosting;
using Fiona.Hosting.Abstractions;

IFionaHostBuilder serviceBuilder = FionaHostBuilder.CreateHostBuilder();

serviceBuilder.Service.AddSingleton<IInterface, Impl>();

using IFionaHost host = serviceBuilder.Build();

host.Run();
```

[Read more about dependency injection](Dependency-injection.md)

## How to add custom middleware

1. Create your middleware

```c#
using System.Net;
using Fiona.Hosting.Abstractions.Middleware;
using Fiona.Hosting.Tests.FionaServer.Mock;

namespace Fiona.Hosting.Tests.FionaServer;

public sealed class CustomMiddleware() : IMiddleware
{
    public async Task Invoke(HttpListenerContext request, NextMiddlewareDelegate next)
    {
        // Here you can write action before rest request

        await next(request);

        // Here you can write action after rest request
    }
}
```

Then add it to the `Program.cs`

```c#
builder.AddMiddleware<CustomMiddleware>();
```

[Read more about middleware](Middleware.md)
