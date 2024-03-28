using Fiona.Hosting.Controller.Attributes;
using Fiona.Hosting.Routing;
using Fiona.Hosting.Routing.Attributes;

namespace Fiona.Hosting.TestServer.Controller;

[Controller("error")]
public class ErrorController
{
    public void Index()
    {
        throw new Exception("test");
    }
}