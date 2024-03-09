using System.Net;

namespace Fiona.Hosting.Abstractions.Middleware;

public interface IMiddleware
{
    public Task Invoke(HttpListenerContext request, NextMiddlewareDelegate next);
}