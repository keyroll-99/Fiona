using Fiona.Hosting.Tests.Utils;
using Fiona.Hosting.Tests.Utils.Controller;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Fiona.Hosting.Tests.InfrastructureTests;

[Collection("FionaTests")]
public class InfrastructureTest(FionaTestServerBuilder testBuilder)
{
    [Fact]
    public void Before_Host_Run_Should_Add_Automatically_Controllers()
    {
        // Arrange
        var provider = testBuilder.Builder.Service;
        
        // Assert
        provider.Any(x => x.ServiceType == typeof(HomeController)).Should().BeTrue();
        provider.Any(x => x.ServiceType == typeof(AboutController)).Should().BeTrue();
    }
}