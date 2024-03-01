using System.Net;

namespace Fiona.Hosting;

public class FionaServer : IDisposable
{
    private readonly HttpListener _httpListener;
    private static bool _isRunning;
    
    public FionaServer(string port)
    {
        _httpListener = new HttpListener();
        _httpListener.Prefixes.Add($"https://localhost:{port}/");
    }
    
    public void Start()
    {
        _isRunning = true;
        _httpListener.Start();
        while (_isRunning && _httpListener.IsListening)
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
        _httpListener.Stop();
    }
    

    public void Dispose()
    {
        _isRunning = false;
        _httpListener.Stop();
    }
}