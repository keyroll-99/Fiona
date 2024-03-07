using System.Net;
using System.Text;
using System.Text.Json;
using Fiona.Hosting.Abstractions;
using Fiona.Hosting.Controller;
using Fiona.Hosting.Models;
using Fiona.Hosting.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Fiona.Hosting;

internal sealed class FionaHost(IServiceProvider serviceProvider, HostConfig config) : IFionaHost
{
    private readonly HttpListener _httpListener = new();

    public void Dispose()
    {
        ((IDisposable)_httpListener).Dispose();
    }

    public void Run()
    {
        ThrowErrorIfServerAlreadyRunning();
        ConfigureListener();
        RunHost().GetAwaiter().GetResult();
    }

    private void ConfigureListener()
    {
        _httpListener.Prefixes.Add($"{config.Url}:{config.Port}/");
    }

    private async Task RunHost()
    {
        _httpListener.Start();
        var router = serviceProvider.GetRequiredService<Router>();

        while (_httpListener.IsListening)
        {
            HttpListenerContext context = await _httpListener.GetContextAsync();
            HttpListenerRequest request = context.Request;


            ObjectResult result = await router.CallEndpoint(request.Url,
                HttpMethodTypeExtensionMethods.GetHttpMethodType(request.HttpMethod) ?? HttpMethodType.Get,
                request.HasEntityBody
                    ? request.InputStream
                    : null);

            Type? resultType = result.Result?.GetType();

            string? responseString = string.Empty;
            if (resultType is not null)
            {
                if (resultType.IsPrimitive || resultType == typeof(string))
                {
                    responseString = result.Result?.ToString();
                }
                else
                {
                    responseString = JsonSerializer.Serialize(result.Result);
                }
            }

            var response = context.Response;

            var buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            response.StatusCode = (int)result.StatusCode;
            var output = response.OutputStream;
            await output.WriteAsync(buffer);
            output.Close();
        }

        _httpListener.Close();
    }

    private void ThrowErrorIfServerAlreadyRunning()
    {
        if (_httpListener.IsListening)
        {
            throw new InvalidOperationException("Server is already running");
        }
    }
}