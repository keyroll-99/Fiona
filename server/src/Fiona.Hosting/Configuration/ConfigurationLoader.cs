using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Fiona.Hosting.Configuration;

public static class ConfigurationLoader
{
    public static AppConfig GetConfig(Assembly assembly)
    {
        var appLocation = Path.GetDirectoryName(assembly.Location);

        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath($"{appLocation}/AppSettings")
            .AddJsonFile("ServerSetting.json", optional: false).Build();
        AppConfig appConfig = new();
        config.Bind(appConfig);
        return appConfig;
    }
}