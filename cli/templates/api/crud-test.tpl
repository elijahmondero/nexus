using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Nexus.Tests.Infrastructure;
using {{Namespace}}.Models;
using Xunit;

namespace Nexus.Tests.Features;

public class {{Name}}Tests : IClassFixture<ApiFixture>
{
    private readonly HttpClient _client;

    public {{Name}}Tests(ApiFixture fixture)
    {
        _client = fixture.CreateClient();
    }

    [Fact]
    public async Task Get{{Name}}s_Returns200()
    {
        // Act
        var response = await _client.GetAsync("/api/{{Name}}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Create{{Name}}_WithValidData_Returns201()
    {
        // Arrange
        var model = new {{Name}} { Name = "Test {{Name}}" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/{{Name}}", model);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}
