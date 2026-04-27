using Microsoft.Playwright;
using Nexus.Tests.Infrastructure;
using Xunit;

namespace Nexus.Tests.Features.E2E;

public class {{Name}}E2ETests : IClassFixture<ApiFixture>
{
    private readonly string _frontendUrl = "http://localhost:3000";

    [Fact]
    public async Task {{Name}}Page_DisplaysScaffoldedData()
    {
        // Arrange
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync();
        var page = await browser.NewPageAsync();

        // Login first
        await page.GotoAsync($"{_frontendUrl}/login?apiUrl={ApiFixture.BrowserUrl}");
        await page.FillAsync("#email", "admin@nexus.dev");
        await page.FillAsync("#password", "admin");
        await page.ClickAsync("button:has-text('Login')");

        // Act - Navigate to the new feature
        await page.GotoAsync($"{_frontendUrl}/{{Name.toLowerCase()}}");

        // Assert
        var header = await page.InnerTextAsync("h4");
        Assert.Equal("{{Name}}", header);
    }
}
