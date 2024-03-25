using System.Net;
using Fiona.Hosting.Controller;
using Fiona.Hosting.Controller.Attributes;
using Fiona.Hosting.Routing;
using Fiona.Hosting.Routing.Attributes;

namespace Fiona.Hosting.TestServer.Controller;

[Controller("status-code")]
public sealed class StatusCodeTest
{
    [Route(HttpMethodType.Get, "200")]
    public Task<IResult> Get200()
    {
        return Task.FromResult<IResult>(new ObjectResult(HttpStatusCode.OK));
    }


    [Route(HttpMethodType.Get, "201")]
    public Task<IResult> Get201()
    {
        return Task.FromResult<IResult>(new ObjectResult(HttpStatusCode.Created));
    }


    [Route(HttpMethodType.Get, "202")]
    public Task<IResult> Get202()
    {
        return Task.FromResult<IResult>(new ObjectResult(HttpStatusCode.Accepted));
    }


    [Route(HttpMethodType.Get, "500")]
    public Task<IResult> Get500()
    {
        return Task.FromResult<IResult>(new ObjectResult(HttpStatusCode.InternalServerError));
    }

    [Route(HttpMethodType.Get, "400")]
    public Task<IResult> Get400()
    {
        return Task.FromResult<IResult>(new ObjectResult(HttpStatusCode.BadRequest));
    }

    [Route(HttpMethodType.Get, "300")]
    public Task<IResult> Get300()
    {
        return Task.FromResult<IResult>(new ObjectResult(HttpStatusCode.Ambiguous));
    }

    [Route(HttpMethodType.Get, "404")]
    public Task<IResult> Get404()
    {
        return Task.FromResult<IResult>(new ObjectResult(HttpStatusCode.NotFound));
    }
}