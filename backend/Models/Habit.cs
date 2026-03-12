using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Clarity.Api.Models;

public class Habit
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = null!;

    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string Frequency { get; set; } = null!; // e.g. Daily, Weekly
    public string Color { get; set; } = null!;

    public int StreakCurrent { get; set; } = 0;
    public int StreakLongest { get; set; } = 0;

    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CreatedAt { get; set; }
}
