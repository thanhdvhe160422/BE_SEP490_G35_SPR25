namespace Planify_BackEnd.DTOs.Events
{
    public class EventDetailResponseDTO
    {
        public class EventDetailDto
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
            public string Placed { get; set; }
            public DateTime CreatedAt { get; set; }
            public string CampusName { get; set; }
            public string CategoryEventName { get; set; }
            public UserDto CreatedBy { get; set; }
            public List<EventMediaDto> EventMedia { get; set; }
            public List<GroupDto> Groups { get; set; }
        }

        public class UserDto
        {
            public Guid Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
        }

        public class EventMediaDto
        {
            public int Id { get; set; }
            public string MediaUrl { get; set; }
        }

        public class GroupDto
        {
            public int Id { get; set; }
            public string GroupName { get; set; }
            public decimal AmountBudget { get; set; }
            public List<JoinGroupDto> JoinGroups { get; set; }
        }

        public class JoinGroupDto
        {
            public int Id { get; set; }
            public Guid ImplementerId { get; set; }
            public string ImplementerFirstName { get; set; }
            public string ImplementerLastName { get; set; }
            public DateTime TimeJoin { get; set; }
            public DateTime? TimeOut { get; set; }
            public int Status { get; set; }
        }
    }
}
