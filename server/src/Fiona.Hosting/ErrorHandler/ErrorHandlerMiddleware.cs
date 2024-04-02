using System.Net;
using System.Text;
using Fiona.Hosting.Abstractions.Configuration;
using Fiona.Hosting.Abstractions.Middleware;
using Fiona.Hosting.Configuration;
using Microsoft.Extensions.Logging;

namespace Fiona.Hosting.ErrorHandler;

internal sealed class ErrorHandlerMiddleware(IOption<ServerConfig> serverConfig, ILogger<ErrorHandlerMiddleware> logger) : IMiddleware
{
    private readonly ServerConfig _serverConfig = serverConfig.Value;

    public async Task Invoke(HttpListenerContext request, NextMiddlewareDelegate next)
    {
        try
        {
           await next(request);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.ToString());
            await HandleError(request, ex);
        }
    }

    private async Task HandleError(HttpListenerContext request, Exception ex)
    {
        HttpListenerResponse response = request.Response;
        response.StatusCode = (int)HttpStatusCode.InternalServerError;
        string message = _serverConfig.ReturnFullErrorMessage ? ex.Message : "Internal Server Error";
        byte[] buffer = Encoding.UTF8.GetBytes(message);
        response.ContentLength64 = buffer.Length;
        Stream output = response.OutputStream;
        await output.WriteAsync(buffer);

        request.Response.Close();
    }
}