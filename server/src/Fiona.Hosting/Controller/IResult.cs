using System.Net;

namespace Fiona.Hosting.Controller;

public interface IResult
{
    public HttpStatusCode StatusCode { get; }
}