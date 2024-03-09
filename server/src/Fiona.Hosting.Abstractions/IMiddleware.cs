using System.Net;

namespace Fiona.Hosting.Abstractions;

public interface IMiddleware
{
    public Task Invoke(HttpListenerContext request, RequestDelegate next);
}