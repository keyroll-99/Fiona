using System.Net;
using System.Text;
using System.Text.Json;
using Fiona.Hosting.Tests.Utils;
using Fiona.Hosting.Tests.Utils.Models;
using FluentAssertions;

namespace Fiona.Hosting.Tests.ApiTests;

[Collection("FionaTests")]
public class GetTests(FionaTestServerBuilder testBuilder)
{
    private FionaTestServerBuilder _testServerBuilder = testBuilder;

    private readonly HttpClient _httpClient = new HttpClient()
    {
        BaseAddress = new Uri("http://localhost:7000/"),
    };

    [Fact]
    public async Task When_Call_Server_Should_Got_Response()
    {
        var response = await _httpClient.GetAsync("");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task When_Call_Home_Page_By_Http_Get_Return_Home_String()
    {
        var response = await _httpClient.GetAsync("");
        var content = await response.Content.ReadAsStringAsync();

        content.Should().Be("Home");
    }

    [Fact]
    public async Task When_Call_About_Page_By_Http_Get_Return_About_String()
    {
        var response = await _httpClient.GetAsync("about");
        var content = await response.Content.ReadAsStringAsync();

        content.Should().Be("About");
    }

    [Fact]
    public async Task When_Call_About_Page_By_Http_Get_Return_About_Another_Route_String()
    {
        var response = await _httpClient.GetAsync("about/another/route");
        var content = await response.Content.ReadAsStringAsync();

        content.Should().Be("another/route");
    }

    [Fact]
    public async Task When_Call_About_Page_By_Http_Post_Return_About_Another_Route_String()
    {
        var response = await _httpClient.GetAsync("about/another/route");
        var content = await response.Content.ReadAsStringAsync();

        content.Should().Be("another/route");
    }

    [Fact]
    public async Task When_Call_About_Page_By_Http_Put_Return_404_Not_Found()
    {
        var response = await _httpClient.PutAsync("about/another/route", new StringContent(""));

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task When_Call_User_By_Http_Get_Return_User_Object()
    {
        var response = await _httpClient.GetAsync("user");
        var content = await response.Content.ReadAsStringAsync();
        var user = JsonSerializer.Deserialize<UserModel>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        });

        user.Should().BeOfType<UserModel>().And.BeEquivalentTo(new UserModel() { Id = 1, Name = "John" });
    }

    [Fact]
    public async Task When_Call_User_By_Http_Post_Should_Return_User_From_Body()
    {
        var user = new UserModel() { Id = 2, Name = "Jane" };
        var json = JsonSerializer.Serialize(user);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("user", data);
        var content = await response.Content.ReadAsStringAsync();
        var userFromResponse = JsonSerializer.Deserialize<UserModel>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        });

        userFromResponse.Should().BeOfType<UserModel>().And.BeEquivalentTo(user);
    }

    [Fact]
    public async Task When_Call_User_By_Http_Put_Should_Return_User_From_Body()
    {
        var user = new UserModel() { Id = 2, Name = "Jane" };
        var json = JsonSerializer.Serialize(user);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync("user", data);
        var content = await response.Content.ReadAsStringAsync();
        var userFromResponse = JsonSerializer.Deserialize<UserModel>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        });

        userFromResponse.Should().BeOfType<UserModel>().And.BeEquivalentTo(user);
    }

    [Fact]
    public async Task When_Call_User_By_Http_Patch_Should_Return_User_From_Body()
    {
        var user = new UserModel() { Id = 2, Name = "Jane" };
        var json = JsonSerializer.Serialize(user);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PatchAsync("user", data);
        var content = await response.Content.ReadAsStringAsync();
        var userFromResponse = JsonSerializer.Deserialize<UserModel>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        });

        userFromResponse.Should().BeOfType<UserModel>().And.BeEquivalentTo(user);
    }

    [Fact]
    public async Task When_Call_User_By_Http_Delete_Should_Return_Empty_Content()
    {
        var response = await _httpClient.DeleteAsync("user?userId=2");
        var content = await response.Content.ReadAsStringAsync();

        content.Should().BeEmpty();
    }

    [Fact]
    public async Task When_Given_User_Props_In_Args_Should_Return_Model()
    {
        var response = await _httpClient.GetAsync("userFromArgs?userId=2&name=Jane");
        var content = await response.Content.ReadAsStringAsync();
        var userFromResponse = JsonSerializer.Deserialize<UserModel>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        });

        var user = new UserModel() { Id = 2, Name = "Jane" };
        userFromResponse.Should().BeOfType<UserModel>().And.BeEquivalentTo(user);
    }

    [Fact]
    public async Task When_Call_Home_By_Http_Post_Should_Return_405_Method_Not_Allowed()
    {
        var response = await _httpClient.PostAsync("", new StringContent(""));

        response.StatusCode.Should().Be(HttpStatusCode.MethodNotAllowed);
    }

    [Fact]
    public async Task When_Call_Admin_By_Http_Get_Should_Return_404_Not_Found()
    {
        var response = await _httpClient.GetAsync("admin");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}