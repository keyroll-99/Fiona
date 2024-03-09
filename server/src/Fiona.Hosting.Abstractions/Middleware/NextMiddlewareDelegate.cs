using System.Net;

namespace Fiona.Hosting.Abstractions.Middleware;

public delegate Task NextMiddlewareDelegate(HttpListenerContext context);
