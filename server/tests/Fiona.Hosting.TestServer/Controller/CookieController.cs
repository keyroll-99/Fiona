using System.Net;
using Fiona.Hosting.Controller;
using Fiona.Hosting.Controller.Attributes;
using Fiona.Hosting.Cookie;
using Fiona.Hosting.Routing;
using Fiona.Hosting.Routing.Attributes;

namespace Fiona.Hosting.TestServer.Controller;

[Controller("cookie")]
public class CookieController
{
    [Route(HttpMethodType.Get, "set")]
    public Task<ObjectResult> SetCookie()
    {
        return Task.FromResult(new ObjectResult(null, HttpStatusCode.OK).SetCookie("Fiona", "Fiona"));
    }

    [Route(HttpMethodType.Get, "get")]
    public Task<ObjectResult> GetCookie([Cookie("fiona")] string? notFiona, [Cookie] string? secondCookie)
    {
        return Task.FromResult(new ObjectResult(new { notFiona, secondCookie }, HttpStatusCode.OK));
    }
}