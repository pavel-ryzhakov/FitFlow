using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FitFlow.Application.Auth;
using FitFlow.Domain.Enums;
using FitFlow.IntegrationTests.Common;

namespace FitFlow.IntegrationTests.Clients;

public class ClientsAuthorizationTests : IClassFixture<FitFlowWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ClientsAuthorizationTests(FitFlowWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetClients_ShouldReturnUnauthorized_WhenTokenIsMissing()
    {
        var response = await _client.GetAsync("/api/clients");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
    
    [Fact]
    public async Task GetClients_ShouldReturnOk_WhenUserHasManagerRole()
    {
        var authResponse = await RegisterUserAsync(UserRole.Manager);

        Assert.False(string.IsNullOrWhiteSpace(authResponse.AccessToken));
        Assert.Equal("Manager", authResponse.Role);

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", authResponse.AccessToken);

        var response = await _client.GetAsync("/api/clients");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private async Task<AuthResponse> RegisterUserAsync(UserRole role)
    {
        var userName = $"user_{Guid.NewGuid():N}";

        var request = new UserRegistrationRequest
        {
            UserName = userName,
            Email = $"{userName}@fitflow.local",
            Password = "123456",
            Role = role
        };

        var response = await _client.PostAsJsonAsync("/api/auth/register", request);

        response.EnsureSuccessStatusCode();

        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();

        return authResponse!;
    }
}