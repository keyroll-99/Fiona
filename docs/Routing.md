# Routing

Routing is defined by the `Route` attribute. An endpoint can be only a public method. If the public method doesn't have a Route attribute that mean the method will be available under the GET method.

For example, the Index method will be w available under `GET /home`

```c#
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

```

## Route attribute args

Route has 3 params

1. HttpMethodType - mandatory, this argument defined with HTTP method will be handled by this method
1. string route - not mandatory, default empty string, this argument defines the route under which it is available.
1. string[] queryParameters - not mandatory, default empty array, this param defines what query parameter can come.

## HTTP method type

Currently, Fiona framework supports this HTTP method type

```C#
[Flags]
public enum HttpMethodType
{
    Get = 1 << 0,
    Post = 1 << 1,
    Put = 1 << 2,
    Patch = 1 << 3,
    Delete = 1 << 4
}
```

One method can handle two or more types of HTTP types. If you want to do this, you have to add a parameter like this
`[Route(HttpMethodType.Post | HttpMethodType.Put)]`

## Route

By default method got the route from the Controller attribute, if you want to add a new part you can just pass a route argument.
For example method index will be available under the`/home/controller` route, because the route from Route attribute always is added to the route from the Controller

```c#
[Controller("home")]
public class HomeController(ILogger<HomeController> logger)
{
    private readonly ILogger<HomeController> _logger = logger;

    [Route(HttpMethodType.Post, "controller")]
    public Task<string> Index()
    {
        _logger.Log(LogLevel.Critical, "Home Controller Index");
        return Task.FromResult("Home");
    }
}

```

## Params from route

To pass a param from route you just have to define it in the Route attribute by wrapping part of the argument by `{}`.
``[Route(HttpMethodType.Post, "{controller}"`)]`
The part between`{}`will be also the name of your argument. If you want to pass it to the method you just have to add an argument which the same name like the name between`{}`

```c#
[Controller("home")]
public class HomeController(ILogger<HomeController> logger)
{
    private readonly ILogger<HomeController> _logger = logger;

    [Route(HttpMethodType.Post, "{controller}")]
    public Task<string> Index(string controller)
    {
        _logger.Log(LogLevel.Critical, "Home Controller Index");
        return Task.FromResult("Home");
    }
}

```

If you will have to route `[Route(HttpMethodType.Get, "/home/{index}")]` and `[Route(HttpMethodType.Get, "/home/index")]` doesn't matter which one is first defined. `GET /home/index` always will be called`/home/index`.

## Query params

To pass a query param to a method. First, you have to declare it in the route attribute. Then you can pass it as a method argument. The name of the argument has to be the same as the name in the route attribute.
In the future is planned to add a QueryParamAttribute as an alternative way.

```c#
    [Route(HttpMethodType.Get, "userFromArgs", ["userId", "name"])]
    public Task<ObjectResult> UserFromArgs(int userId, string name)
    {
        return Task.FromResult(
            new ObjectResult(
                new UserModel
                {
                    Id = userId, Name = name
                },
                HttpStatusCode.OK
            )
        );
    }
```
