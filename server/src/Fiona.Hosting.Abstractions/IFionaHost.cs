namespace Fiona.Hosting.Abstractions;

public interface IFionaHost : IDisposable
{
    void Run();
}