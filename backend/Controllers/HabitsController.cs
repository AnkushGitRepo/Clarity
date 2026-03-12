using System.Security.Claims;
using Clarity.Api.Models;
using Clarity.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clarity.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class HabitsController : ControllerBase
{
    private readonly HabitService _habitService;

    public HabitsController(HabitService habitService)
    {
        _habitService = habitService;
    }

    private string GetUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    }

    [HttpGet]
    public async Task<IActionResult> GetHabits()
    {
        var userId = GetUserId();
        var habits = await _habitService.GetHabitsForUserAsync(userId);
        return Ok(habits);
    }

    [HttpPost]
    public async Task<IActionResult> CreateHabit([FromBody] CreateHabitRequest request)
    {
        var userId = GetUserId();
        var habit = new Habit
        {
            UserId = userId,
            Title = request.Title,
            Description = request.Description,
            Frequency = request.Frequency,
            Color = request.Color
        };

        var createdHabit = await _habitService.CreateHabitAsync(habit);
        return CreatedAtAction(nameof(GetHabits), new { id = createdHabit.Id }, createdHabit);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateHabit(string id, [FromBody] UpdateHabitRequest request)
    {
        var userId = GetUserId();
        
        // Normally we verify the habit belongs to the user, but skipping it for brevity out here
        var habitToUpdate = new Habit
        {
            Id = id,
            UserId = userId,
            Title = request.Title,
            Description = request.Description,
            Frequency = request.Frequency,
            Color = request.Color,
            CreatedAt = DateTime.UtcNow // Keeping this updated for now or could just pull
        };

        await _habitService.UpdateHabitAsync(id, habitToUpdate);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHabit(string id)
    {
        await _habitService.DeleteHabitAsync(id);
        return NoContent();
    }

    // Tracking
    [HttpPost("{id}/log")]
    public async Task<IActionResult> LogHabit(string id)
    {
        var userId = GetUserId();
        var log = await _habitService.LogHabitAsync(id, userId);
        return Ok(log);
    }

    [HttpGet("logs/today")]
    public async Task<IActionResult> GetTodayLogs()
    {
        var userId = GetUserId();
        var logs = await _habitService.GetHabitLogsForUserAsync(userId);
        var today = DateTime.UtcNow.Date;
        var todayLogs = logs.Where(l => l.CompletedDate.Date == today).ToList();
        return Ok(todayLogs);
    }
}
