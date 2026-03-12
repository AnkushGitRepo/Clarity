using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Clarity.Api.Models;

public class HabitLog
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string HabitId { get; set; } = null!;

    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = null!;

    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CompletedDate { get; set; } // The date the habit was completed (typically date portion only)

    public bool Status { get; set; } // True if done
}
