using System.Net;
using FluentAssertions;

namespace Fiona.Hosting.Tests.Cookie;

[Collection("FionaTests")]
public class CookieTests
{
    


    [Fact]
    public async Task Should_Return_Cookie_When_Call_Cookie_Set()
    {
        // Arrange
        CookieContainer cookies = new();
        HttpClientHandler handler = new();
        
        // Act
        HttpClient httpClient = new(handler)
        {
            BaseAddress = new Uri("http://localhost:7000/"),
        };
        
        HttpResponseMessage response = await httpClient.GetAsync("/cookie/set");
        Uri uri = new("http://localhost:7000");
        IEnumerable<System.Net.Cookie> responseCookies = cookies.GetCookies(uri);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseCookies.Where(x => x.Name == "Fiona").Should().NotBeEmpty();
        responseCookies.Where(x => x.Name == "Fiona").First().Value.Should().Be("Fiona");
    }
}