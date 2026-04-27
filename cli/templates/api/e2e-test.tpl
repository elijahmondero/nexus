using Microsoft.Playwright;
using Nexus.Tests.Infrastructure;
using Xunit;

namespace Nexus.Tests.Features.E2E;

public class {{Name}}E2ETests : IClassFixture<ApiFixture>
{
    private readonly string _frontendUrl = "http://127.0.0.1:3000";

    [Fact]
    public async Task {{Name}}Page_DisplaysScaffoldedData()
    {
        // Arrange
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync();
        var page = await browser.NewPageAsync();

        page.Console += (_, e) => Console.WriteLine($"Browser Console: {e.Text}");
        page.PageError += (_, e) => Console.WriteLine($"Browser Error: {e}");

        // Login first
        await page.GotoAsync($"{_frontendUrl}/login?apiUrl={ApiFixture.BrowserUrl}");
        await page.Locator("input#email").FillAsync("admin@nexus.dev");
        await page.Locator("input#password").FillAsync("admin");
        await page.ClickAsync("button[type='submit']");

        // Wait for login to complete and navigate to dashboard
        await page.WaitForSelectorAsync("h4:has-text('Welcome to Nexus')", new PageWaitForSelectorOptions { Timeout = 10000 });

        // Act - Navigate to the new feature
        await page.GotoAsync($"{_frontendUrl}/{{Name.toLowerCase()}}?apiUrl={ApiFixture.BrowserUrl}");

        // Assert - wait for header to be visible
        await page.WaitForSelectorAsync("h4:has-text('{{Name}}')", new PageWaitForSelectorOptions { Timeout = 10000 });
        var header = await page.InnerTextAsync("h4");
        Assert.Equal("{{Name}}", header);

        // Optionally interact with the new Create button
        await page.ClickAsync("button:has-text('Create {{Name}}')");
    }
}
