using System.Net;
using System.Text;
using System.Text.Json;
using Fiona.Hosting.Abstractions;
using Fiona.Hosting.Controller;
using Fiona.Hosting.Middleware;
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

        while (_httpListener.IsListening)
        {
            HttpListenerContext context = await _httpListener.GetContextAsync();
            using var scope = serviceProvider.CreateScope();
            MiddlewareCallStack callStack = scope.ServiceProvider.GetRequiredService<MiddlewareCallStack>();
            await callStack.Invoke(context);
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