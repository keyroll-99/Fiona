using System.Net;
using System.Text.Json;
using Fiona.Hosting.TestServer.Models;
using FluentAssertions;
using JsonSerializerOptions = System.Text.Json.JsonSerializerOptions;

namespace Fiona.Hosting.Tests.OptionTests;

[Collection("FionaTests")]
public class OptionTest
{
    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("http://localhost:7000/"),
    };
    
    [Fact]
    public async Task Should_Return_Correct_Config_When_Call_Option_Get()
    {
        // Act
        var response = await _httpClient.GetAsync("option/get");
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        var appConfig = JsonSerializer.Deserialize<ConfigModel>(content, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
        });
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        appConfig.Should().NotBeNull();
        appConfig.Name.Should().Be("Config");
        appConfig.Version.Should().Be("1.1");
    }
}