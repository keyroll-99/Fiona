using System.Net;
using Fiona.Hosting.Controller;
using Fiona.Hosting.Routing;
using Fiona.Hosting.Tests.FionaServer.Models;

namespace Fiona.Hosting.Tests.FionaServer.Controller;

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
    
    [Route(HttpMethodType.Post, "/another")]
    public ObjectResult CreateAnother(UserModel user)
    {
        return new ObjectResult(user, HttpStatusCode.Created);
    }
    
    [Route(HttpMethodType.Patch, "/another")]
    public Task<ObjectResult> PatchAnother(UserModel user)
    {
        return Task.FromResult(new ObjectResult(user, HttpStatusCode.OK));
    }
    
    
    [Route(HttpMethodType.Put, "/another")]
    public Task<ObjectResult> PutAnother(UserModel user)
    {
        return Task.FromResult(new ObjectResult(user, HttpStatusCode.OK));
    }
    
    [Route(HttpMethodType.Post, "/param/{id}")]
    public Task<UserModel> UserAndParams(int id, UserModel user)
    {
        return Task.FromResult(new UserModel
        {
            Id = id, Name = user.Name
        });
    }    
    
    [Route(HttpMethodType.Get, "/param/{id}/and/{name}")]
    public Task<UserModel> UserAndTwoParams(int id, string name)
    {
        return Task.FromResult(new UserModel
        {
            Id = id, Name = name
        });
    }  
    
    [Route(HttpMethodType.Get, "/param/id/and/name")]
    public Task<UserModel> UserAndOneParams()
    {
        return Task.FromResult(new UserModel
        {
            Id = 21, Name = "Jan"
        });
    }
}