namespace Fiona.Hosting.Configuration;

internal sealed class ServerConfig
{
    public string Domain { get; set; } = "http://localhost";
    public string Port { get; set; } = "7001";
    public string Environment { get; set; } = "Development";
    public bool ReturnFullErrorMessage { get; set; }
}