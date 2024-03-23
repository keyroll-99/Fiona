using Fiona.Hosting.Abstractions.Configuration;

namespace Fiona.Hosting.Configuration;

internal readonly struct Option<T>(T value) : IOption<T>
{
    public T Value { get; } = value;
}