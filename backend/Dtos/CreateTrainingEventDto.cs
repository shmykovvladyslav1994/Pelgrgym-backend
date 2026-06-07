using backend.Models;

namespace backend.Dtos
{
    public class CreateTrainingEventDto
    {
        public TrainingResult Result { get; set; }
        public int TrainingId { get; set; }
    }
}
