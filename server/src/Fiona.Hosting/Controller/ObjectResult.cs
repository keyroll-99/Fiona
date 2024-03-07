using System.Net;

namespace Fiona.Hosting.Controller;

public sealed class ObjectResult(object? result, HttpStatusCode statusCode) : IResult
{
    public object? Result { get; } = result;
    public HttpStatusCode StatusCode { get; } = statusCode;
}