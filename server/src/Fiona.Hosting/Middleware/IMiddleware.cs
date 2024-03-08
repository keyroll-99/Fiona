using System.Net;

namespace Fiona.Hosting.Middleware;

public interface IMiddleware
{
    public Task Invoke(HttpListenerContext request, Delegate next);
}