using System.Net;

namespace Fiona.Hosting.Controller;

public sealed class ObjectResult(object? result, HttpStatusCode statusCode) : IResult
{
    public object? Result { get; } = result;
    public HttpStatusCode StatusCode { get; } = statusCode;

    public static ObjectResult FromObject(object? result, HttpStatusCode statusCode = HttpStatusCode.OK) =>
        new(result, statusCode);
}