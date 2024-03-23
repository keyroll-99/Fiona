namespace Fiona.Hosting.Configuration;

public sealed class AppConfig
{
    public string Domain { get; set; } = "http://127.0.0.1";
    public string Port { get; set; } = "7001";
    public string Environment { get; set; } = "Development";
}