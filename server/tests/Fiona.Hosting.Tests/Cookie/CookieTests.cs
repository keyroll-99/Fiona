using System.Net;
using FluentAssertions;

namespace Fiona.Hosting.Tests.Cookie;

[Collection("FionaTests")]
public class CookieTests
{
    


    [Fact]
    public async Task Should_Return_Cookie_When_Call_Cookie_Set()
    {
        // Act
        HttpClient httpClient = new()
        {
            BaseAddress = new Uri("http://localhost:7000/"),
        };
        
        HttpResponseMessage response = await httpClient.GetAsync("/cookie/set");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Headers.Where(x => x.Key.ToLowerInvariant() == "set-cookie").Should().NotBeEmpty();
        response.Headers.Where(x => x.Key.ToLowerInvariant() == "set-cookie").First().Value.First().Should().StartWith("Fiona=Fiona;");
    }
}