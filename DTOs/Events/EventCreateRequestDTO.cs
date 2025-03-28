using System.ComponentModel.DataAnnotations;

namespace Planify_BackEnd.DTOs.Events
{
    public class EventCreateRequestDTO
    {
        public string EventTitle { get; set; }
        public string EventDescription { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal AmountBudget { get; set; }
        public int CategoryEventId { get; set; }
        public string Placed { get; set; }
        public string? MeasuringSuccess { get; set; }
        public string? Goals { get; set; }
        public string? MonitoringProcess { get; set; }
        public int? SizeParticipants { get; set; }
        public List<TaskEventCreateRequestDTO>? Tasks { get; set; }
        public List<RiskCreateRequestDTO>? Risks { get; set; }
    }

    public class TaskEventCreateRequestDTO
    {
        public string TaskName { get; set; }
        public string? Description { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime Deadline { get; set; }
        public decimal Budget { get; set; }
        public List<SubEventTaskCreateRequestDTO>? SubTasks { get; set; }
    }

    public class SubEventTaskCreateRequestDTO
    {
        public string SubTaskName { get; set; }
        public string? Description { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime Deadline { get; set; }
        public decimal Budget { get; set; }
    }

    public class RiskCreateRequestDTO
    {
        public string Name { get; set; }
        public string? Reason { get; set; }
        public string? Solution { get; set; }
        public string? Description { get; set; }
    }
}
