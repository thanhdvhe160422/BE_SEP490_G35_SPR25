using System.ComponentModel.DataAnnotations;

namespace Planify_BackEnd.DTOs.Events
{
    public class EventCreateRequestDTO
    {
        [Required]
        public string EventTitle { get; set; } = null!;

        public string? EventDescription { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        public decimal AmountBudget { get; set; }

        [Required]
        public int CategoryEventId { get; set; }

        public string? Placed { get; set; }

        public Guid CreateBy { get; set; }

        public List<GroupCreateDTO>? Groups { get; set; } = new();

        public List<string>? EventMediaUrls { get; set; }

        public EventCreateRequestDTO() { }
    }

    public class GroupCreateDTO
    {
        [Required]
        public string GroupName { get; set; } = null!;
        public Guid CreateBy { get; set; }
        public int EventId { get; set; }
        public List<Guid> ImplementerIds { get; set; } = new();
    }
}
