using Clarity.Api.Models;
using Clarity.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Clarity.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var user = await _authService.RegisterUserAsync(request.Username, request.Email, request.Password);
            return Ok(new { message = "Registration successful" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var token = await _authService.LoginAsync(request.Email, request.Password);
        
        if (token == null)
            return Unauthorized(new { message = "Invalid email or password" });

        var user = await _authService.GetUserByEmailAsync(request.Email);
        
        return Ok(new AuthResponse { Token = token, Username = user!.Username });
    }
}
