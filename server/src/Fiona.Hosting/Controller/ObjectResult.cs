using System.Net;

namespace Fiona.Hosting.Controller;

public sealed class ObjectResult(object? result, HttpStatusCode statusCode) : IResult
{
    public ObjectResult(HttpStatusCode statusCode) : this(null, statusCode)
    {
    }

    public object? Result { get; } = result;
    public Dictionary<string, string> Cookies { get; } = new();
    public HttpStatusCode StatusCode { get; } = statusCode;

    public ObjectResult SetCookie(string key, string value)
    {
        Cookies.Add(key, value);
        return this;
    }
}