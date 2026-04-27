using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Nexus.Api.Auth.Models;
using Nexus.Tests.Infrastructure;
using Xunit;

namespace Nexus.Tests.Features;

public class AuthTests : IClassFixture<ApiFixture>
{
    private readonly HttpClient _client;

    public AuthTests(ApiFixture fixture)
    {
        _client = fixture.CreateClient();
    }

    [Fact]
    public async Task Login_WithValidCredentials_Returns200AndToken()
    {
        // Arrange
        var request = new LoginRequest 
        { 
            Email = "admin@nexus.dev", 
            Password = "admin" 
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        result.Should().NotBeNull();
        result!.Token.Should().NotBeNullOrEmpty();
        result.Email.Should().Be(request.Email);
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_Returns401()
    {
        // Arrange
        var request = new LoginRequest 
        { 
            Email = "wrong@nexus.dev", 
            Password = "wrong" 
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
