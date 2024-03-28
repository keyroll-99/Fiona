using System.Net;
using FluentAssertions;

namespace Fiona.Hosting.Tests.ErrorHandler;

[Collection("FionaTests")]
public class ErrorHandlerTests
{
    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("http://localhost:7000/")
    };

    
    [Fact]
    public async Task Should_Got_500_Internal_Server_Error()
    {
        // Act
        var response = await _httpClient.GetAsync("error");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
}