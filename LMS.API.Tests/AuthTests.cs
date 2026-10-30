using System.Net;
using System.Net.Http.Json;

namespace LMS.API.Tests;

public class AuthTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Courses_without_token_returns_401()
    {
        var response = await _client.GetAsync("/api/courses");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Student_logs_in_returns_200()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/login", new
        {
            email = "Student@Lms.com",
            password = ApiFactory.SeedPassword
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Wrong_password_returns_401()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/login", new
        {
            email = "Student@Lms.com",
            password = "wrong-password"
        });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
