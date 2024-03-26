using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Fiona.Hosting.Configuration;

public static class ConfigurationLoader
{
    public static AppConfig GetConfig(Assembly assembly)
    {
        string? appLocation = Path.GetDirectoryName(assembly.Location);

        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath($"{appLocation}/AppSettings")
            .AddJsonFile("ServerSetting.json", false).Build();
        AppConfig appConfig = new();
        config.Bind(appConfig);
        return appConfig;
    }
}