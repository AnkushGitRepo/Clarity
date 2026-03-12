using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Clarity.Api.Models;

public class PomodoroSession
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string? UserId { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string? HabitId { get; set; } // Optional tag to a habit

    public int DurationMinutes { get; set; } // Length of the focus session

    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CompletedAt { get; set; }
}
