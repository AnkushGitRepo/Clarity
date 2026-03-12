namespace Clarity.Api.Models;

public class CreateHabitRequest
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string Frequency { get; set; } = null!;
    public string Color { get; set; } = null!;
}

public class UpdateHabitRequest
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string Frequency { get; set; } = null!;
    public string Color { get; set; } = null!;
}
