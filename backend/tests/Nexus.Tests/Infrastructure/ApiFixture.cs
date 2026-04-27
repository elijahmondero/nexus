using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Nexus.Api.Data.Migrations;
using Testcontainers.PostgreSql;
using Xunit;

namespace Nexus.Tests.Infrastructure;

public class ApiFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:16-alpine")
        .WithDatabase("nexus_test")
        .WithUsername("nexus")
        .WithPassword("password")
        .Build();

    // Use 0.0.0.0 so it's reachable via any host alias (localhost, 127.0.0.1)
    public const string ServerUrl = "http://0.0.0.0:5001";
    // The URL the browser should use to reach the host
    public const string BrowserUrl = "http://127.0.0.1:5001";

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        MigrationRunner.Run(_dbContainer.GetConnectionString(), "postgres");
        
        // This ensures the server starts on the real port
        _ = Server; 
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseUrls(ServerUrl);
        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["DB_PROVIDER"] = "postgres",
                ["DB_CONNECTION_STRING"] = _dbContainer.GetConnectionString(),
                ["JWT_SECRET"] = "super_secret_test_key_32_characters_long",
                ["JWT_ISSUER"] = "nexus-api",
                ["JWT_AUDIENCE"] = "nexus-client"
            });
        });
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }
}
