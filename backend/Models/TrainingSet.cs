public class TrainingSet
{
    public int Id { get; set; }

    public int Reps { get; set; }

    public string ProgressionType { get; set; } = null!;
    public int ProgressionValue { get; set; }

    public int TrainingId { get; set; }
    public Training Training { get; set; } = null!;
}