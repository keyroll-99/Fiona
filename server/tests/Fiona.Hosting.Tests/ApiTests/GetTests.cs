using System.Net;
using System.Text;
using System.Text.Json;
using Fiona.Hosting.Tests.FionaServer;
using Fiona.Hosting.TestServer.Models;
using FluentAssertions;

namespace Fiona.Hosting.Tests.ApiTests;

[Collection("FionaTests")]
public class GetTests(FionaTestServerBuilder testBuilder)
{
    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("http://localhost:7000/")
    };

    private FionaTestServerBuilder _testServerBuilder = testBuilder;

    [Fact]
    public async Task When_Call_Server_Should_Got_Response()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task When_Call_Home_Page_By_Http_Get_Return_Home_String()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("");
        string content = await response.Content.ReadAsStringAsync();

        content.Should().Be("Home");
    }

    [Fact]
    public async Task When_Call_About_Page_By_Http_Get_Return_About_String()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("about");
        string content = await response.Content.ReadAsStringAsync();

        content.Should().Be("About");
    }

    [Fact]
    public async Task When_Call_About_Page_By_Http_Get_Return_About_Another_Route_String()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("about/another/route");
        string content = await response.Content.ReadAsStringAsync();

        content.Should().Be("another/route");
    }

    [Fact]
    public async Task When_Call_About_Page_By_Http_Post_Return_About_Another_Route_String()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("about/another/route");
        string content = await response.Content.ReadAsStringAsync();

        content.Should().Be("another/route");
    }

    [Fact]
    public async Task When_Call_About_Page_By_Http_Put_Return_404_Not_Found()
    {
        HttpResponseMessage response = await _httpClient.PutAsync("about/another/route", new StringContent(""));

        response.StatusCode.Should().Be(HttpStatusCode.MethodNotAllowed);
    }

    [Fact]
    public async Task When_Call_User_By_Http_Get_Return_User_Object()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("user");
        string content = await response.Content.ReadAsStringAsync();
        UserModel? user = JsonSerializer.Deserialize<UserModel>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        user.Should().BeOfType<UserModel>().And.BeEquivalentTo(new UserModel { Id = 1, Name = "John" });
    }

    [Fact]
    public async Task When_Call_User_By_Http_Post_Should_Return_User_From_Body()
    {
        UserModel user = new UserModel { Id = 2, Name = "Jane" };
        string json = JsonSerializer.Serialize(user);
        StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _httpClient.PostAsync("user", data);
        string content = await response.Content.ReadAsStringAsync();
        UserModel? userFromResponse = JsonSerializer.Deserialize<UserModel>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        userFromResponse.Should().BeOfType<UserModel>().And.BeEquivalentTo(user);
    }

    [Fact]
    public async Task When_Call_User_By_Http_Put_Should_Return_User_From_Body()
    {
        UserModel user = new UserModel { Id = 2, Name = "Jane" };
        string json = JsonSerializer.Serialize(user);
        StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _httpClient.PutAsync("user", data);
        string content = await response.Content.ReadAsStringAsync();
        UserModel? userFromResponse = JsonSerializer.Deserialize<UserModel>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        userFromResponse.Should().BeOfType<UserModel>().And.BeEquivalentTo(user);
    }

    [Fact]
    public async Task When_Call_User_By_Http_Patch_Should_Return_User_From_Body()
    {
        UserModel user = new UserModel { Id = 2, Name = "Jane" };
        string json = JsonSerializer.Serialize(user);
        StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _httpClient.PatchAsync("user", data);
        string content = await response.Content.ReadAsStringAsync();
        UserModel? userFromResponse = JsonSerializer.Deserialize<UserModel>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        userFromResponse.Should().BeOfType<UserModel>().And.BeEquivalentTo(user);
    }

    [Fact]
    public async Task When_Call_User_By_Http_Delete_Should_Return_Empty_Content()
    {
        HttpResponseMessage response = await _httpClient.DeleteAsync("user?userId=2");
        string content = await response.Content.ReadAsStringAsync();

        content.Should().BeEmpty();
    }

    [Fact]
    public async Task
        When_Given_User_Props_And_Method_Doesnt_Have_Route_Attribute_Then_Should_Math_Args_And_Return_Model()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("user/userFromArgs?userId=2&name=Jane");
        string content = await response.Content.ReadAsStringAsync();
        UserModel? userFromResponse = JsonSerializer.Deserialize<UserModel>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        UserModel user = new UserModel { Id = 2, Name = "Jane" };
        userFromResponse.Should().BeOfType<UserModel>().And.BeEquivalentTo(user);
    }

    [Fact]
    public async Task When_Given_User_Props_And_Method_Has_Route_Attribute_Then_Should_Use_Attribute_And_Return_Model()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("user/userFromArgs/second?userId=2&name=Jane");
        string content = await response.Content.ReadAsStringAsync();
        UserModel? userFromResponse = JsonSerializer.Deserialize<UserModel>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        UserModel user = new UserModel { Id = 2, Name = "Jane" };
        userFromResponse.Should().BeOfType<UserModel>().And.BeEquivalentTo(user);
    }

    [Fact]
    public async Task When_Call_Home_By_Http_Post_Should_Return_405_Method_Not_Allowed()
    {
        HttpResponseMessage response = await _httpClient.PostAsync("", new StringContent(""));

        response.StatusCode.Should().Be(HttpStatusCode.MethodNotAllowed);
    }

    [Fact]
    public async Task When_Call_Admin_By_Http_Get_Should_Return_404_Not_Found()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("admin");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task When_Call_User_Another_By_Http_Post_Should_Return_201_Created()
    {
        UserModel user = new UserModel { Id = 2, Name = "Jane" };
        string json = JsonSerializer.Serialize(user);
        StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _httpClient.PostAsync("user/another", data);
        string content = await response.Content.ReadAsStringAsync();
        UserModel? userFromResponse = JsonSerializer.Deserialize<UserModel>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        userFromResponse.Should().BeOfType<UserModel>().And.BeEquivalentTo(user);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task When_Call_User_Another_By_Http_Put_Should_Return_200_OK()
    {
        UserModel user = new UserModel { Id = 2, Name = "Jane" };
        string json = JsonSerializer.Serialize(user);
        StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _httpClient.PutAsync("user/another", data);
        string content = await response.Content.ReadAsStringAsync();
        UserModel? userFromResponse = JsonSerializer.Deserialize<UserModel>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        userFromResponse.Should().BeOfType<UserModel>().And.BeEquivalentTo(user);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task When_Call_User_Another_By_Http_Patch_Should_Return_200_OK()
    {
        UserModel user = new UserModel { Id = 2, Name = "Jane" };
        string json = JsonSerializer.Serialize(user);
        StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _httpClient.PatchAsync("user/another", data);
        string content = await response.Content.ReadAsStringAsync();
        UserModel? userFromResponse = JsonSerializer.Deserialize<UserModel>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        userFromResponse.Should().BeOfType<UserModel>().And.BeEquivalentTo(user);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task When_Url_Has_One_Parameter_Then_Should_Call_Endpoint_With_Parameter()
    {
        //Arrange
        UserModel user = new UserModel { Id = 2, Name = "Jane" };
        string json = JsonSerializer.Serialize(user);
        StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        HttpResponseMessage response = await _httpClient.PostAsync("user/param/1", data);
        string content = await response.Content.ReadAsStringAsync();
        UserModel? userFromResponse = JsonSerializer.Deserialize<UserModel>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        // Assert
        userFromResponse!.Id.Should().Be(1);
        userFromResponse.Name.Should().Be(user.Name);
    }

    [Fact]
    public async Task When_Url_Has_Many_Parameter_Then_Should_Call_Endpoint_With_Parameter()
    {
        // Act
        HttpResponseMessage response = await _httpClient.GetAsync("user/param/1/and/jhon");
        string content = await response.Content.ReadAsStringAsync();
        UserModel? userFromResponse = JsonSerializer.Deserialize<UserModel>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        // Assert
        userFromResponse!.Name.Should().Be("jhon");
        userFromResponse.Id.Should().Be(1);
    }

    [Fact]
    public async Task Should_Firs_Call_Unparameterized_Url()
    {
        // Act
        HttpResponseMessage response = await _httpClient.GetAsync("user/param/id/and/name");
        string content = await response.Content.ReadAsStringAsync();
        UserModel? userFromResponse = JsonSerializer.Deserialize<UserModel>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        // Assert
        userFromResponse!.Name.Should().Be("Jan");
        userFromResponse.Id.Should().Be(21);
    }

    [Fact]
    public async Task When_Two_Route_With_Different_Parameter_Should_Call_Correct_Endpoint()
    {
        // Act
        HttpResponseMessage response = await _httpClient.GetAsync("user/param/marek/and/id");
        string content = await response.Content.ReadAsStringAsync();
        UserModel? userFromResponse = JsonSerializer.Deserialize<UserModel>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        // Assert
        userFromResponse!.Name.Should().Be("marek");
        userFromResponse.Id.Should().Be(21);
    }
}