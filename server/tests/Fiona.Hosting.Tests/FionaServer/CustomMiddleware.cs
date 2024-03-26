using System.Net;
using Fiona.Hosting.Abstractions.Middleware;
using Fiona.Hosting.Tests.FionaServer.Mock;

namespace Fiona.Hosting.Tests.FionaServer;

public sealed class CustomMiddleware(ICallMock callMock) : IMiddleware
{
    public async Task Invoke(HttpListenerContext request, NextMiddlewareDelegate next)
    {
        await callMock.Call();

        await next(request);

        await callMock.Call();
    }
}