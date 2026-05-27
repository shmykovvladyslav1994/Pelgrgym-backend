namespace backend.Models
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public bool IsDone { get; internal set; }
    }
}

namespace backend.Models
    {
        public class CreatedTaskResponse
        {
            public Guid Id { get; set; }
        }
    }  