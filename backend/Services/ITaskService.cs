using Clarity.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Clarity.Api.Services;

public interface ITaskService
{
    Task<List<TaskItem>> GetTasksAsync(string userId);
    Task<TaskItem?> GetTaskAsync(string id, string userId);
    Task<TaskItem> CreateTaskAsync(TaskItem task);
    Task UpdateTaskAsync(string id, TaskItem taskIn);
    Task RemoveTaskAsync(string id, string userId);
}
