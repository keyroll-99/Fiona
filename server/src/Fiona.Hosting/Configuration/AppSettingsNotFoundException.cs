namespace Fiona.Hosting.Configuration;

public class AppSettingsNotFoundException() : Exception("AppSettings/AppSettings.json not found.")
{
    public static void ThrowIfNotFound(string? appSettingsLocation)
    {
        if (appSettingsLocation is null) throw new AppSettingsNotFoundException();
    }
}