using Backend.Models;

public class CreateTrainingDto
{
    public string Name { get; set; } = null!;
    public int RestIntervalSec { get; set; }
    public IncrementOrder IncrementOrder { get; set; }

    public TrainingCycleDto TrainingCycle { get; set; } = null!;

    public List<SetDto> Sets { get; set; } = new();
}