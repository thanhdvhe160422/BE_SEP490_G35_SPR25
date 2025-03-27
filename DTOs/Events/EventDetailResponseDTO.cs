namespace Planify_BackEnd.DTOs.Events
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
            public List<TaskDetailDto> Tasks { get; set; }
        }

        public class TaskDetailDto
        {
            public int Id { get; set; }
            public string TaskName { get; set; }
            public string TaskDescription { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime? Deadline { get; set; }
            public decimal AmountBudget { get; set; }
            public DateTime CreatedAt { get; set; }
            public UserDto CreatedBy { get; set; }
            public List<SubTaskDetailDto> SubTasks { get; set; }
        }

        public class SubTaskDetailDto
        {
            public int Id { get; set; }
            public string SubTaskName { get; set; }
            public string SubTaskDescription { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime? Deadline { get; set; }
            public decimal AmountBudget { get; set; }
            public DateTime CreatedAt { get; set; }
            public UserDto CreatedBy { get; set; }
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
    }