using System.ComponentModel.DataAnnotations;

namespace Planify_BackEnd.DTOs.Events
{
    public class EventCreateRequestDTO
    {
        [Required]
        public string EventTitle { get; set; } = null!;
        [Required]
        public string EventDescription { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }
        [Required]
        public decimal AmountBudget { get; set; }

        [Required]
        public int CategoryEventId { get; set; }
        [Required]
        public string Placed { get; set; }

        public List<TaskEventCreateRequestDTO> Tasks { get; set; }

    }

    public class TaskEventCreateRequestDTO
    {
        public string TaskName { get; set; }
        public string Description { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime Deadline { get; set; }
        public decimal Budget { get; set; }
        public List<SubTaskEventCreateRequestDTO> SubTasks { get; set; }
    }

    public class SubTaskEventCreateRequestDTO
    {
        public string SubTaskName { get; set; }
        public string Description { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime Deadline { get; set; }
        public decimal Budget { get; set; }
    }
}
