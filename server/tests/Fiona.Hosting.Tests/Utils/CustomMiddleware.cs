using System.Net;
using Fiona.Hosting.Middleware;

namespace Fiona.Hosting.Tests.Utils;

public sealed class CustomMiddleware : IMiddleware
{
    public async Task Invoke(HttpListenerContext request, Delegate next)
    { 
        
    }
}