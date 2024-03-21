using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Fiona.Hosting.Configuration;

public static class ConfigurationLoader
{
    public static HostConfig GetConfig(Assembly assembly)
    {
        var appLocation = Path.GetDirectoryName(assembly.Location);

        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath($"{appLocation}/AppSettings")
            .AddJsonFile("ServerSetting.json", optional: false).Build();
        HostConfig hostConfig = new();
        config.Bind(hostConfig);
        return hostConfig;
    }
}