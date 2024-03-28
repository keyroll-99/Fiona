using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Fiona.Hosting.Configuration;

internal static class ConfigurationLoader
{
    public static ServerConfig GetConfig(Assembly assembly)
    {
        string? appLocation = Path.GetDirectoryName(assembly.Location);

        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath($"{appLocation}/AppSettings")
            .AddJsonFile("ServerSetting.json", false).Build();
        ServerConfig serverConfig = new();
        config.Bind(serverConfig);
        return serverConfig;
    }
}