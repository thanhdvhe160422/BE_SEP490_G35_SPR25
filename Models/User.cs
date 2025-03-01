using System;
using System.Collections.Generic;

namespace Planify_BackEnd.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string Password { get; set; } = null!;

    public DateTime? DateOfBirth { get; set; }

    public string? PhoneNumber { get; set; }

    public int? WardId { get; set; }

    public int? DistrictId { get; set; }

    public int? ProvinceId { get; set; }

    public int? Avatar { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? Role { get; set; }

    public int? CampusId { get; set; }

    public int? Status { get; set; }

    public virtual ICollection<AssignTask> AssignTasks { get; set; } = new List<AssignTask>();

    public virtual MediaItem? AvatarNavigation { get; set; }

    public virtual Campus? Campus { get; set; }

    public virtual District? District { get; set; }

    public virtual ICollection<Event> EventCreateByNavigations { get; set; } = new List<Event>();

    public virtual ICollection<Event> EventManagers { get; set; } = new List<Event>();

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    public virtual ICollection<JoinGroup> JoinGroups { get; set; } = new List<JoinGroup>();

    public virtual ICollection<JoinProject> JoinProjects { get; set; } = new List<JoinProject>();

    public virtual ICollection<JoinTask> JoinTasks { get; set; } = new List<JoinTask>();

    public virtual Province? Province { get; set; }

    public virtual ICollection<Report> ReportReportUsers { get; set; } = new List<Report>();

    public virtual ICollection<Report> ReportSendFromNavigations { get; set; } = new List<Report>();

    public virtual Role? RoleNavigation { get; set; }

    public virtual ICollection<SendRequest> SendRequests { get; set; } = new List<SendRequest>();

    public virtual ICollection<SubTask> SubTasks { get; set; } = new List<SubTask>();

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    public virtual Ward? Ward { get; set; }
}
