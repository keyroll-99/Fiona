using System.Net;
using Fiona.Hosting.Abstractions.Middleware;

namespace Fiona.Hosting.Middleware;

internal sealed class MiddlewareCallStack
{
    private readonly Queue<IMiddleware> _middlewaresStack = new();

    public MiddlewareCallStack(IEnumerable<IMiddleware> middlewares)
    {
        foreach (IMiddleware middleware in middlewares) _middlewaresStack.Enqueue(middleware);
    }

    public async Task Invoke(HttpListenerContext context)
    {
        if (_middlewaresStack.Count == 0) return;

        IMiddleware middleware = _middlewaresStack.Dequeue();
        await middleware.Invoke(context, Invoke);
    }
}