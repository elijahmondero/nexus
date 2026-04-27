using Microsoft.AspNetCore.Mvc;
using Nexus.Api.Auth.Models;

namespace Nexus.Api.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (request.Email == "admin@nexus.dev" && request.Password == "admin")
        {
            var token = _authService.GenerateToken(request.Email);
            return Ok(new LoginResponse { Token = token, Email = request.Email });
        }

        return Unauthorized();
    }
}
