using System.ComponentModel.DataAnnotations;

namespace Planify_BackEnd.DTOs.Tasks
{
    public class TaskUpdateRequestDTO
    {
        public string TaskName { get; set; }

        public string TaskDescription { get; set; }

        public DateTime CreateDate { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime Deadline { get; set; }
        public decimal AmountBudget { get; set; }

        public double Progress { get; set; }

        public int Status { get; set; }
        public TaskUpdateRequestDTO() { }
    }
}
