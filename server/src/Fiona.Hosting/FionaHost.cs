using System.Net;
using Fiona.Hosting.Abstractions;
using Fiona.Hosting.Models;
using Fiona.Hosting.Routing;

namespace Fiona.Hosting;

internal sealed class FionaHost(IServiceProvider serviceProvider, HostConfig config, Router router) : IFionaHost
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

    private Task RunHost()
    {
        _httpListener.Start();
        while (_httpListener.IsListening)
        {
            var context = _httpListener.GetContext();
            var response = context.Response;
            var responseString = "test";
            var buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            var output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
        }
        
        _httpListener.Close();
        return Task.CompletedTask; // Tmp, I will be use async to hanlde requests
    }

    private void ThrowErrorIfServerAlreadyRunning()
    {
        if (_httpListener.IsListening)
        {
            throw new InvalidOperationException("Server is already running");
        }
    }
}