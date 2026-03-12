using System.Security.Claims;
using Clarity.Api.Models;
using Clarity.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clarity.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PomodoroController : ControllerBase
{
    private readonly HabitService _habitService;

    public PomodoroController(HabitService habitService)
    {
        _habitService = habitService;
    }

    [HttpPost("log")]
    public async Task<IActionResult> LogSession([FromBody] PomodoroSession request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        request.UserId = userId; // Ensure the session is tied to the authenticated user

        var loggedSession = await _habitService.LogPomodoroSessionAsync(request);
        return Ok(loggedSession);
    }

    [HttpGet("sessions")]
    public async Task<IActionResult> GetSessions()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var sessions = await _habitService.GetPomodoroSessionsForUserAsync(userId);
        return Ok(sessions);
    }
}
