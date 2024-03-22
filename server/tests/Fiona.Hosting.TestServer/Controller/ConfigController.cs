using System.Net;
using Fiona.Hosting.Controller;
using Fiona.Hosting.TestServer.Models;

namespace Fiona.Hosting.TestServer.Controller;

[Controller]
public class ConfigController
{
    private readonly ConfigModel _configModel;
    
    public ConfigController(ConfigModel configModel)
    {
        _configModel = configModel;
    }

    public ObjectResult GetConfig()
    {
        return new ObjectResult(_configModel, HttpStatusCode.OK);
    }
}