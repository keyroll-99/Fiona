namespace Fiona.Hosting.Abstractions;

public interface IFionaHostBuilder
{
    IFionaHost Build();
    
    IFionaHostBuilder SetUrl(string url);
    IFionaHostBuilder SetPort(string url);
}