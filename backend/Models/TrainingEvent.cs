namespace backend.Models;

public class TrainingEvent
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public TrainingResult Result { get; set; }

    // связь с тренировкой
    public int TrainingId { get; set; }
}
