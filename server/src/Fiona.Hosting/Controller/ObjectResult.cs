using System.Net;

namespace Fiona.Hosting.Controller;

public sealed class ObjectResult(object? result, HttpStatusCode statusCode) : IResult
{
    public object? Result { get; } = result;
    public HttpStatusCode StatusCode { get; } = statusCode;
    public Dictionary<string, string> Cookies { get; } = new();
    
    
    public ObjectResult(HttpStatusCode statusCode) : this(null, statusCode)
    {
        
    }
    
    public ObjectResult SetCookie(string key, string value)
    {
        Cookies.Add(key, value);
        return this;
    }
}