using System.Net;
using Fiona.Hosting.Abstractions;
using Fiona.Hosting.Middleware;
using Fiona.Hosting.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Fiona.Hosting;

internal sealed class FionaHost : IFionaHost
{
    private readonly HttpListener _httpListener = new();
    private readonly List<Task> _requestThreads;
    private readonly HostConfig _config;
    private readonly IServiceProvider _serviceProvider;

    public FionaHost(IServiceProvider serviceProvider, HostConfig config)
    {
        _serviceProvider = serviceProvider;
        _config = config;

        ThreadPool.GetMaxThreads(out int maxThreads, out _);
        _requestThreads = new List<Task>(maxThreads);
    }

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
        _httpListener.Prefixes.Add($"{_config.Url}:{_config.Port}/");
    }

    private async Task RunHost()
    {
        _httpListener.Start();

        while (_httpListener.IsListening)
        {
            HttpListenerContext context = await _httpListener.GetContextAsync();

            ThreadPool.GetAvailableThreads(out var worker_tmp, out var io_tmp);

            _requestThreads.Add(Task.Factory.StartNew(async () =>
            {
                using var scope = _serviceProvider.CreateScope();
                MiddlewareCallStack callStack = scope.ServiceProvider.GetRequiredService<MiddlewareCallStack>();
                await callStack.Invoke(context);
            }));
            ThreadPool.GetAvailableThreads(out var worker, out var io);
            _requestThreads.RemoveAll(x => x.IsCompleted);
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