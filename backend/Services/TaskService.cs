using Clarity.Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Clarity.Api.Services;

public class TaskService : ITaskService
{
    private readonly IMongoCollection<TaskItem> _tasks;

    public TaskService(IOptions<MongoDBSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionURI);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _tasks = database.GetCollection<TaskItem>(settings.Value.TasksCollectionName);
    }

    public async Task<List<TaskItem>> GetTasksAsync(string userId)
    {
        return await _tasks.Find(t => t.UserId == userId).ToListAsync();
    }

    public async Task<TaskItem?> GetTaskAsync(string id, string userId)
    {
        return await _tasks.Find(t => t.Id == id && t.UserId == userId).FirstOrDefaultAsync();
    }

    public async Task<TaskItem> CreateTaskAsync(TaskItem task)
    {
        await _tasks.InsertOneAsync(task);
        return task;
    }

    public async Task UpdateTaskAsync(string id, TaskItem taskIn)
    {
        await _tasks.ReplaceOneAsync(t => t.Id == id && t.UserId == taskIn.UserId, taskIn);
    }

    public async Task RemoveTaskAsync(string id, string userId)
    {
        await _tasks.DeleteOneAsync(t => t.Id == id && t.UserId == userId);
    }
}
