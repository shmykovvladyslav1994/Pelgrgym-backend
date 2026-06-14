namespace backend.Models;

public class TrainingEvent
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public TrainingResult Result { get; set; }

    public int TrainingId { get; set; }

    public Training Training { get; set; } = null!;
}