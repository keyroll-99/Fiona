namespace Fiona.Hosting.Abstractions.Configuration;

public interface IOption<T>
{
    T Value { get; }
}