using Fiona.Hosting.Controller;
using Fiona.Hosting.Routing;
using Fiona.Hosting.Tests.Utils.Models;

namespace Fiona.Hosting.Tests.Utils.Controller;

[Controller("user")]
public sealed class UserController
{
    public UserModel Index()
    {
        return new UserModel
        {
            Id = 1, Name = "John"
        };
    }
    
    [Route(HttpMethodType.Post | HttpMethodType.Patch | HttpMethodType.Put)]
    public UserModel Create(UserModel user)
    {
        return user;
    }
}