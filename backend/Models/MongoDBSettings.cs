namespace Clarity.Api.Models;

public class MongoDBSettings
{
    public string ConnectionURI { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string UsersCollectionName { get; set; } = null!;
    public string HabitsCollectionName { get; set; } = null!;
    public string HabitLogsCollectionName { get; set; } = null!;
    public string PomodoroSessionsCollectionName { get; set; } = null!;
    public string TasksCollectionName { get; set; } = null!;
}
