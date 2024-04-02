# Middleware

Middleware part of code which is responsible for handling requests and responses.

## How to create middleware

To create an interface you have to implement interface `IMiddleware`.Then in the body of `Invoke` method, you can write your logic

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

Then you have to register your middleware in the app.

```c#
builder.AddMiddleware<CustomMiddleware>();
```

Middleware is like a stack, the first one registered is called the first

## Build-in middlewares

There are two built-in middleware.

1. ErrorHandlerMiddleware - which is always called at the beginning. It is responsible for handling all errors which will be thrown during the execution.
2. CallEndpointMiddleware - which is responsible for finding the correct method and invoking it. It is always called as last middleware. After calling this middleware you cannot update the response in the request.
