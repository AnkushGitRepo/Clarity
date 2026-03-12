using Clarity.Api.Models;
using Clarity.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Clarity.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    private string GetUserId()
    {
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(id)) throw new System.Exception("User ID not found in token");
        return id;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var userId = GetUserId();
        var tasks = await _taskService.GetTasksAsync(userId);
        return Ok(tasks);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateTaskRequest request)
    {
        var userId = GetUserId();
        var newTask = new TaskItem
        {
            UserId = userId,
            Title = request.Title,
            Description = request.Description,
            Type = request.Type,
            ParentTaskId = request.ParentTaskId,
            Deadline = request.Deadline,
            Color = request.Color
        };

        await _taskService.CreateTaskAsync(newTask);
        return CreatedAtAction(nameof(Get), new { id = newTask.Id }, newTask);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromBody] UpdateTaskRequest request)
    {
        var userId = GetUserId();
        var existingTask = await _taskService.GetTaskAsync(id, userId);

        if (existingTask == null)
            return NotFound();

        if (request.Title != null) existingTask.Title = request.Title;
        if (request.Description != null) existingTask.Description = request.Description;
        if (request.Type != null) existingTask.Type = request.Type;
        if (request.ParentTaskId != null) existingTask.ParentTaskId = request.ParentTaskId;
        if (request.Deadline != null) existingTask.Deadline = request.Deadline;
        if (request.Color != null) existingTask.Color = request.Color;
        if (request.IsCompleted.HasValue) existingTask.IsCompleted = request.IsCompleted.Value;

        await _taskService.UpdateTaskAsync(id, existingTask);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var userId = GetUserId();
        var task = await _taskService.GetTaskAsync(id, userId);

        if (task == null)
            return NotFound();

        await _taskService.RemoveTaskAsync(id, userId);
        return NoContent();
    }
}
