# Fiona

---
![logo](assets/logo.jpg)

---
Fiona is a web api framework written in .Net. As a developer, you will be able to create API easily and quickly.
The planned key feature is a GUI where a developer will be able to create API using blocks like a blueprint in UE.

In the beginning, I used HttpListener, but in the distant future, I want to write my HTTP handshake.

The project was initiated as part of the "100 commits" competition

The Repo will be renamed after 100 commits competition because I came up with the idea for the name after registering the repo.

## Is it ready on production
Nope, maybe in the future

## Road map

Must to have - I want to finish it by to end of march
- [X] Server
	- [X] Folder structure 
    - [X] First Tests
	- [X] Simple HTTP server with host
	- [X] Startup builder
	- [X] Routing
	- [X] Simple Controller
	- [X] Parsing body to object
	- [X] Middleware
	- [X] Passing parameters and args from the route
	- [X] Response (status codes etc)
	- [X] Async handle request
	- [X] Configuration like a baseUrl, port ETC
	- [X] Cookies
	- [X] Logger
	- [X] Error handler
	- [X] Documentation :)
- [X] Tools
    - [X] Github actions to build it

Good to have - 30-60 days
- [ ] GUI
	- [X] Simple view where the user can drag and drop boxes
	- [ ] Nav menu
	- [ ] Creating project
	- [ ] Users can connect boxes
	- [ ] Run project
	- [ ] Adding controller 
	- [ ] Adding service
	- [ ] Add a way to create simple CRUD
	- [ ] Add logic statement
	- [ ] Add loop statement

Nice to have - 30 days
- [ ] Tools
	- [ ] Docker image
- [ ] Server
    - [ ] File upload
	- [ ] Inject service to a controller method
	- [ ] Auto impl register
	- [ ] Auto inject to method
	- [ ] Extension for EF
	- [ ] Security
		- [ ] CORS
	- [ ] Before call action, as an attribute
	- [ ] Support for plain text in body
- [ ] GUI
	- [ ] Database configuration

Would to have
- [ ] Debugger
- [ ] Own http handshake
- [ ] Cache
- [ ] HttpClient - user can easily use it
- [ ] Add user login option


## How to run a simple server
in `Program.cs`
```c# 
using Fiona.Hosting;
using Fiona.Hosting.Abstractions;

IFionaHostBuilder serviceBuilder = FionaHostBuilder.CreateHostBuilder();

using IFionaHost host = serviceBuilder.Build();

host.Run();
```

sample controller

```c#
using Fiona.Hosting.Controller.Attributes;
using Fiona.Hosting.Routing;
using Fiona.Hosting.Routing.Attributes;
using Microsoft.Extensions.Logging;

namespace SampleFionaServer.Controller;

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

if you want to read more click [here](./docs/Readme.md)