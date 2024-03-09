using Fiona.Hosting.Tests.Utils;
using Fiona.Hosting.Tests.Utils.Controller;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Fiona.Hosting.Tests.InfrastructureTests;

[Collection("FionaTests")]
public class InfrastructureTest(FionaTestServerBuilder testBuilder)
{
    private readonly HttpClient _httpClient = new HttpClient()
    {
        BaseAddress = new Uri("http://localhost:7000/"),
    };
    
    [Fact]
    public async Task Before_Host_Run_Should_Add_Automatically_Controllers()
    {
        // Arrange
        var provider = testBuilder.Builder.Service;
        
        // Assert
        provider.Any(x => x.ServiceType == typeof(HomeController)).Should().BeTrue();
        provider.Any(x => x.ServiceType == typeof(AboutController)).Should().BeTrue();
        provider.Any(x => x.ServiceType == typeof(UserController)).Should().BeTrue();
    }
    
    
    [Fact]
    public async Task Should_Call_Middleware_When_Request_Is_Received()
    {
        // Arrange
        var response = await _httpClient.GetAsync("");
        
        // Assert
        testBuilder.CallMock.Received(2);
    }
}