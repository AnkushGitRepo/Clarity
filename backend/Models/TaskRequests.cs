using System;

namespace Clarity.Api.Models
{
    public class CreateTaskRequest
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public required string Type { get; set; } 
        public string? ParentTaskId { get; set; } 
        public DateTime? Deadline { get; set; }
        public string? Color { get; set; }
    }

    public class UpdateTaskRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
        public string? ParentTaskId { get; set; }
        public DateTime? Deadline { get; set; }
        public string? Color { get; set; }
        public bool? IsCompleted { get; set; }
    }
}
