using Clarity.Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Clarity.Api.Services;

public class HabitService
{
    private readonly IMongoCollection<Habit> _habitsCollection;
    private readonly IMongoCollection<HabitLog> _habitLogsCollection;
    private readonly IMongoCollection<PomodoroSession> _pomodoroCollection;

    public HabitService(IOptions<MongoDBSettings> mongoDBSettings)
    {
        var mongoClient = new MongoClient(mongoDBSettings.Value.ConnectionURI);
        var mongoDatabase = mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _habitsCollection = mongoDatabase.GetCollection<Habit>(mongoDBSettings.Value.HabitsCollectionName);
        _habitLogsCollection = mongoDatabase.GetCollection<HabitLog>(mongoDBSettings.Value.HabitLogsCollectionName);
        _pomodoroCollection = mongoDatabase.GetCollection<PomodoroSession>(mongoDBSettings.Value.PomodoroSessionsCollectionName);
    }

    public async Task<List<Habit>> GetHabitsForUserAsync(string userId)
    {
        return await _habitsCollection.Find(h => h.UserId == userId).ToListAsync();
    }

    public async Task<Habit> CreateHabitAsync(Habit habit)
    {
        habit.CreatedAt = DateTime.UtcNow;
        await _habitsCollection.InsertOneAsync(habit);
        return habit;
    }

    public async Task UpdateHabitAsync(string id, Habit updatedHabit)
    {
        await _habitsCollection.ReplaceOneAsync(h => h.Id == id, updatedHabit);
    }

    public async Task DeleteHabitAsync(string id)
    {
        await _habitsCollection.DeleteOneAsync(h => h.Id == id);
        // Optionally delete associated logs
        await _habitLogsCollection.DeleteManyAsync(l => l.HabitId == id);
    }

    // Tracking and Analytics
    public async Task<HabitLog> LogHabitAsync(string habitId, string userId)
    {
        var today = DateTime.UtcNow.Date;
        
        var existingLog = await _habitLogsCollection.Find(l => 
            l.HabitId == habitId && 
            l.UserId == userId && 
            l.CompletedDate == today
        ).FirstOrDefaultAsync();

        if (existingLog != null)
        {
            existingLog.Status = !existingLog.Status;
            await _habitLogsCollection.ReplaceOneAsync(l => l.Id == existingLog.Id, existingLog);
            await UpdateStreaksAsync(habitId);
            return existingLog;
        }

        var newLog = new HabitLog
        {
            HabitId = habitId,
            UserId = userId,
            CompletedDate = today,
            Status = true
        };

        await _habitLogsCollection.InsertOneAsync(newLog);
        await UpdateStreaksAsync(habitId);
        return newLog;
    }

    private async Task UpdateStreaksAsync(string habitId)
    {
        var habit = await _habitsCollection.Find(h => h.Id == habitId).FirstOrDefaultAsync();
        if (habit == null) return;

        var logs = await _habitLogsCollection.Find(l => l.HabitId == habitId && l.Status == true)
                                             .SortByDescending(l => l.CompletedDate)
                                             .ToListAsync();

        var completedDates = logs.Select(l => l.CompletedDate.Date).Distinct().ToList();

        if (!completedDates.Any())
        {
            habit.StreakCurrent = 0;
            await _habitsCollection.ReplaceOneAsync(h => h.Id == habitId, habit);
            return;
        }

        var today = DateTime.UtcNow.Date;
        var yesterday = today.AddDays(-1);

        int currentStreak = 0;
        DateTime checkDate = completedDates.First();

        // If the most recent completion is neither today nor yesterday, the streak is broken
        if (checkDate != today && checkDate != yesterday)
        {
            currentStreak = 0;
        }
        else
        {
            currentStreak = 1;
            for (int i = 1; i < completedDates.Count; i++)
            {
                if (completedDates[i] == checkDate.AddDays(-1))
                {
                    currentStreak++;
                    checkDate = completedDates[i];
                }
                else
                {
                    break;
                }
            }
        }

        habit.StreakCurrent = currentStreak;
        if (currentStreak > habit.StreakLongest)
        {
            habit.StreakLongest = currentStreak;
        }

        // Normally you'd recalculate the absolute longest streak from all historical data to be purely accurate
        // but updating the watermark here is sufficient for this MVP.
        await _habitsCollection.ReplaceOneAsync(h => h.Id == habitId, habit);
    }

    public async Task<List<HabitLog>> GetHabitLogsForUserAsync(string userId)
    {
        return await _habitLogsCollection.Find(l => l.UserId == userId).ToListAsync();
    }

    public async Task<PomodoroSession> LogPomodoroSessionAsync(PomodoroSession session)
    {
        session.CompletedAt = DateTime.UtcNow;
        await _pomodoroCollection.InsertOneAsync(session);
        return session;
    }

    public async Task<List<PomodoroSession>> GetPomodoroSessionsForUserAsync(string userId)
    {
        return await _pomodoroCollection.Find(s => s.UserId == userId).SortByDescending(s => s.CompletedAt).ToListAsync();
    }
}
