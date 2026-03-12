using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Clarity.Api.Models
{
    public class TaskItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public required string UserId { get; set; }

        public required string Title { get; set; }

        public string? Description { get; set; }

        // Monthly, Weekly, Daily
        public required string Type { get; set; } 

        // For nested tasks, linking a Daily task to a Weekly/Monthly parent
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ParentTaskId { get; set; } 

        public DateTime? Deadline { get; set; }

        public string? Color { get; set; }

        public bool IsCompleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
