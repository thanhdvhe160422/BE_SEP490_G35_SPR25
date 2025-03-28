using Planify_BackEnd.Models;

namespace Planify_BackEnd.DTOs.Events
{
    public class EventDTO
    {
        public int Id { get; set; }

        public string EventTitle { get; set; }

        public string EventDescription { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public decimal AmountBudget { get; set; }

        public int IsPublic { get; set; }

        public DateTime? TimePublic { get; set; }

        public int Status { get; set; }

        public string CampusName { get; set; }

        public string CategoryEventName { get; set; }

        public string Placed { get; set; }
        public int? CategoryEventId { get; set; }

        public Guid? UpdateBy { get; set; }

        public string? MeasuringSuccess { get; set; }

        public string? Goals { get; set; }

        public string? MonitoringProcess { get; set; }

        public int? SizeParticipants { get; set; }
    }
}
