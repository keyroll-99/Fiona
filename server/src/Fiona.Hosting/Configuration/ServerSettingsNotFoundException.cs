namespace Fiona.Hosting.Configuration;

internal class ServerSettingsNotFoundException() : Exception("AppSettings/AppSettings.json not found.")
{
    public static void ThrowIfNotFound(string? appSettingsLocation)
    {
        if (appSettingsLocation is null) throw new ServerSettingsNotFoundException();
    }
}