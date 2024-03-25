using System.Net;
using FluentAssertions;

namespace Fiona.Hosting.Tests.Cookie;

[Collection("FionaTests")]
public class CookieTests
{
    readonly HttpClient httpClient = new()
    {
        BaseAddress = new Uri("http://localhost:7000/"),
    };
    
    [Fact]
    public async Task Should_Return_Cookie_When_Call_Cookie_Set()
    {
        // Act
        HttpResponseMessage response = await httpClient.GetAsync("/cookie/set");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Headers.Where(x => x.Key.ToLowerInvariant() == "set-cookie").Should().NotBeEmpty();
        response.Headers.Where(x => x.Key.ToLowerInvariant() == "set-cookie").First().Value.First().Should().StartWith("Fiona=Fiona;");
    }

    [Fact]
    public async Task Should_Return_Cookie_Given_In_Request()
    {
        // Arrange
        const string fionaCookieValue = "Fiona";
        const string secondCookieValue = "SecondCookie";
        var baseAddress = new Uri("http://localhost:7000/");
        var cookieContainer = new CookieContainer();
        using var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
        using var client = new HttpClient(handler) { BaseAddress = baseAddress };
        cookieContainer.Add(baseAddress, new System.Net.Cookie("fiona", fionaCookieValue));
        cookieContainer.Add(baseAddress, new System.Net.Cookie("secondCookie", secondCookieValue));
        
        // Act
        var result = await client.GetAsync("/cookie/get");
        
        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}