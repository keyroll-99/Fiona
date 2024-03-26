using Fiona.Hosting.Abstractions.Configuration;
using Microsoft.Extensions.Configuration;

namespace Fiona.Hosting.Configuration;

internal class OptionFactory(IConfiguration configuration)
{
    public IOption<T> CreateOption<T>() where T : new()
    {
        return CreateOption<T>(typeof(T).Name);
    }

    public IOption<T> CreateOption<T>(string section) where T : new()
    {
        T config = new();
        configuration.GetSection(section).Bind(config);
        return new Option<T>(config);
    }
}