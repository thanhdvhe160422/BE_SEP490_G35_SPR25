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

        public List<GroupCreateDTO>? Groups { get; set; } = new();

        public EventCreateRequestDTO() { }
    }

    public class GroupCreateDTO
    {
        [Required]
        public string GroupName { get; set; } = null!;
        public List<Guid> ImplementerIds { get; set; } = new();
    }
}
