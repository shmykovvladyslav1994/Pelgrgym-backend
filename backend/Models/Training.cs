using backend.Models;
using Backend.Models;

public class Training
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    public int RestIntervalSec { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // cycle (ПЛОСКО)
    public int WorkDays { get; set; }
    public int RestDays { get; set; }

    public IncrementOrder IncrementOrder { get; set; }
    public int IncrementValue { get; set; }
    public int IncrementIntervalPerDays { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public List<TrainingSet> Sets { get; set; } = new();
}