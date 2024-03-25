using System.Net;
using Fiona.Hosting.Abstractions.Configuration;
using Fiona.Hosting.Controller;
using Fiona.Hosting.Controller.Attributes;
using Fiona.Hosting.Routing;
using Fiona.Hosting.Routing.Attributes;
using Fiona.Hosting.TestServer.Models;

namespace Fiona.Hosting.TestServer.Controller;

[Controller]
public class ConfigController(IOption<ConfigModel> configModelOption)
{
    private IOption<ConfigModel>_configModel = configModelOption;

    [Route(HttpMethodType.Get, "option/get")]
    public ObjectResult GetConfig()
    {
        return new ObjectResult(_configModel.Value, HttpStatusCode.OK);
    }
}