using Fiona.Hosting.Tests.FionaServer;
using Fiona.Hosting.TestServer.Controller;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Fiona.Hosting.Tests.InfrastructureTests;

[Collection("FionaTests")]
public class InfrastructureTest(FionaTestServerBuilder testBuilder)
{
    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("http://localhost:7000/"),
    };
    
    [Fact]
    public Task Before_Host_Run_Should_Add_Automatically_Controllers()
    {
        // Arrange
        var provider = testBuilder.FionaTestServerStartup.Builder.Service;
        
        // Assert
        provider.Any(x => x.ServiceType == typeof(HomeController)).Should().BeTrue();
        provider.Any(x => x.ServiceType == typeof(AboutController)).Should().BeTrue();
        provider.Any(x => x.ServiceType == typeof(UserController)).Should().BeTrue();
        return Task.CompletedTask;
    }
    
    
    [Fact]
    public async Task Should_Call_Middleware_When_Request_Is_Received()
    {
        // Arrange
        await _httpClient.GetAsync("");
        await _httpClient.GetAsync("");
        
        // Assert
        await testBuilder.CallMock.Received(4).Call();
    }
}