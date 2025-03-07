namespace Planify_BackEnd.DTOs.JoinedProjects
{
    public class JoinedProjectDTO
    {
        public int Id { get; set; }

        public int? EventId { get; set; }

        public Guid? UserId { get; set; }

        public DateTime? TimeJoinProject { get; set; }

        public DateTime? TimeOutProject { get; set; }

        public int Role { get; set; }
    }
}
