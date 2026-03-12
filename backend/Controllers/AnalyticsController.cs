using System.Security.Claims;
using Clarity.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clarity.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly HabitService _habitService;

    public AnalyticsController(HabitService habitService)
    {
        _habitService = habitService;
    }

    private string GetUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    }

    [HttpGet("heatmap")]
    public async Task<IActionResult> GetHeatmap()
    {
        var userId = GetUserId();
        var logs = await _habitService.GetHabitLogsForUserAsync(userId);
        
        // Group by CompletedDate to count how many habits were done each day
        var heatmapData = logs
            .Where(l => l.Status)
            .GroupBy(l => l.CompletedDate.Date)
            .Select(g => new
            {
                Date = g.Key.ToString("yyyy-MM-dd"),
                Count = g.Count()
            })
            .OrderBy(h => h.Date)
            .ToList();

        return Ok(heatmapData);
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var userId = GetUserId();
        var habits = await _habitService.GetHabitsForUserAsync(userId);
        var logs = await _habitService.GetHabitLogsForUserAsync(userId);

        var totalHabits = habits.Count;
        var totalCompleted = logs.Count(l => l.Status);

        return Ok(new
        {
            TotalHabits = totalHabits,
            TotalCompletions = totalCompleted
        });
    }
}
