using System.Net;
using Fiona.Hosting.Tests.FionaServer;
using FluentAssertions;

namespace Fiona.Hosting.Tests.ApiTests;

[Collection("FionaTests")]
public sealed class StatusCodeTest(FionaTestServerBuilder testBuilder)
{
    private FionaTestServerBuilder _testServerBuilder = testBuilder;

    private readonly HttpClient _httpClient = new HttpClient()
    {
        BaseAddress = new Uri("http://localhost:7000/"),
    };
    
    
    [Theory]
    [InlineData(200)]
    [InlineData(201)]
    [InlineData(202)]
    [InlineData(500)]
    [InlineData(400)]
    [InlineData(300)]
    [InlineData(404)]
    public async Task Should_Got_Correct_Status_Code_From_Api(int statusCode)
    {
        // Act
        var response = await _httpClient.GetAsync($"status-code/{statusCode}");
        
        // Assert
        response.StatusCode.Should().Be((HttpStatusCode)statusCode);
    }
}