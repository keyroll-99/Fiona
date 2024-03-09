using System.Net;
using Fiona.Hosting.Abstractions;
using Fiona.Hosting.Middleware;
using Fiona.Hosting.Tests.Utils.Mock;
using FluentAssertions;

namespace Fiona.Hosting.Tests.Utils;

public sealed class CustomMiddleware(ICallMock callMock) : IMiddleware
{
    public async Task Invoke(HttpListenerContext request, RequestDelegate next)
    {
        await callMock.Call();
        
        await next(request);

        await callMock.Call();
    }
}