using System.Reflection;
using System.Text.Json;

namespace Fiona.Hosting.Configuration;

public static class ConfigurationLoader
{
    public static HostConfig GetConfig(Assembly assembly)
    {
        var appLocation = Path.GetDirectoryName(assembly.Location);
        var appSettingsLocation = Directory.GetFiles(appLocation!, "AppSettings.json", SearchOption.AllDirectories).FirstOrDefault();
        AppSettingsNotFoundException.ThrowIfNotFound(appSettingsLocation);
        
        HostConfig config = JsonSerializer.Deserialize<HostConfig>(File.ReadAllText(appSettingsLocation!))!;

        return config;
    }
}