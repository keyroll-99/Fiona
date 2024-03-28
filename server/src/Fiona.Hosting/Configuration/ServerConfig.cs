namespace Fiona.Hosting.Configuration;

internal sealed class ServerConfig
{
    public string Domain { get; set; } = "http://127.0.0.1";
    public string Port { get; set; } = "7001";
    public string Environment { get; set; } = "Development";
    public bool ReturnFullErrorMessage { get; set; }
}