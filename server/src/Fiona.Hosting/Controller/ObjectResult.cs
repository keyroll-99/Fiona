using System.Net;

namespace Fiona.Hosting.Controller;

public sealed class ObjectResult<T>(T? result, HttpStatusCode statusCode) : IResult
{
    private T? Result { get; } = result;
    public HttpStatusCode StatusCode { get; } = statusCode;
}