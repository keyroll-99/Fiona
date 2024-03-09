using System.Net;

namespace Fiona.Hosting.Abstractions;

public delegate Task RequestDelegate(HttpListenerContext context);
