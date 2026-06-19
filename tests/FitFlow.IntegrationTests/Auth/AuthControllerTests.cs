using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FitFlow.Application.Auth;
using FitFlow.Domain.Enums;
using FitFlow.IntegrationTests.Common;

namespace FitFlow.IntegrationTests.Auth;

public class AuthControllerTests : IClassFixture<FitFlowWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthControllerTests(FitFlowWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_ShouldReturnOkAndAccessToken_WhenRequestIsValid()
    {
        var request = new UserRegistrationRequest
        {
            UserName = $"admin_{Guid.NewGuid():N}",
            Email = $"admin_{Guid.NewGuid():N}@fitflow.local",
            Password = "123456",
            Role = UserRole.Admin
        };

        var response = await _client.PostAsJsonAsync("/api/auth/register", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();

        Assert.NotNull(authResponse);
        Assert.False(string.IsNullOrWhiteSpace(authResponse.AccessToken));
        Assert.Equal(request.UserName, authResponse.UserName);
        Assert.Equal(request.Email, authResponse.Email);
        Assert.Equal("Admin", authResponse.Role);
    }

    [Fact]
    public async Task Login_ShouldReturnOkAndAccessToken_WhenCredentialsAreValid()
    {
        var userName = $"manager_{Guid.NewGuid():N}";
        var password = "123456";

        var registrationRequest = new UserRegistrationRequest
        {
            UserName = userName,
            Email = $"{userName}@fitflow.local",
            Password = password,
            Role = UserRole.Manager
        };

        await _client.PostAsJsonAsync("/api/auth/register", registrationRequest);

        var loginRequest = new UserLoginRequest
        {
            UserNameOrEmail = userName,
            Password = password
        };

        var response = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();

        Assert.NotNull(authResponse);
        Assert.False(string.IsNullOrWhiteSpace(authResponse.AccessToken));
        Assert.Equal(userName, authResponse.UserName);
        Assert.Equal("Manager", authResponse.Role);
    }

    [Fact]
    public async Task Login_ShouldReturnUnauthorized_WhenPasswordIsInvalid()
    {
        var loginRequest = new UserLoginRequest
        {
            UserNameOrEmail = "unknown-user",
            Password = "wrong-password"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}