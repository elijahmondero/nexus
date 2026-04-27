using Microsoft.Playwright;
using Nexus.Tests.Infrastructure;
using Xunit;

namespace Nexus.Tests.Features.E2E;

public class LoginE2ETests : IClassFixture<ApiFixture>
{
    private readonly string _frontendUrl = "http://127.0.0.1:3000";

    [Fact]
    public async Task Login_WithValidCredentials_NavigatesToHomePage()
    {
        // Arrange
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });
        
        var page = await browser.NewPageAsync();
        
        page.Console += (_, e) => Console.WriteLine($"Browser Console: {e.Text}");
        page.PageError += (_, e) => Console.WriteLine($"Browser Error: {e}");

        // Act
        try 
        {
            await page.GotoAsync($"{_frontendUrl}/login?apiUrl={ApiFixture.BrowserUrl}");
            await page.WaitForSelectorAsync("form", new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible, Timeout = 5000 });
        }
        catch (Exception ex)
        {
            var content = await page.ContentAsync();
            throw new Exception($"Failed to load login page. Current content: {content}", ex);
        }

        await page.Locator("input#email").FillAsync("admin@nexus.dev");
        await page.Locator("input#password").FillAsync("admin");
        
        // Wait for the response and potential navigation
        await page.ClickAsync("button[type='submit']");

        // Verify successful login by checking for content that appears on the dashboard/home
        // instead of waiting for a strict URL load which might be interrupted by API mismatch
        try 
        {
            await page.WaitForSelectorAsync("h4:has-text('Welcome to Nexus')", new PageWaitForSelectorOptions { Timeout = 10000 });
        }
        catch (Exception ex)
        {
             var content = await page.ContentAsync();
             throw new Exception($"Failed to find Welcome text after login. Current content: {content}", ex);
        }
        
        var welcomeText = await page.InnerTextAsync("h4");
        Assert.Contains("Welcome to Nexus", welcomeText);
        
        var userEmail = await page.GetByTestId("user-email").InnerTextAsync();
        Assert.Contains("admin@nexus.dev", userEmail);
    }
}
