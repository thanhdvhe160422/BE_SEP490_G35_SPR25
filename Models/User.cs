﻿using System;
using System.Collections.Generic;

namespace Planify_BackEnd.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string? UserName { get; set; }

    public string Email { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Password { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public int? AddressId { get; set; }

    public int? AvatarId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int CampusId { get; set; }

    public int Status { get; set; }

    public virtual Address? Address { get; set; }

    public virtual Medium? Avatar { get; set; }

    public virtual Campus Campus { get; set; } = null!;

    public virtual ICollection<Event> EventCreateByNavigations { get; set; } = new List<Event>();

    public virtual ICollection<Event> EventManagers { get; set; } = new List<Event>();

    public virtual ICollection<Event> EventUpdateByNavigations { get; set; } = new List<Event>();

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    public virtual ICollection<JoinGroup> JoinGroups { get; set; } = new List<JoinGroup>();

    public virtual ICollection<JoinProject> JoinProjects { get; set; } = new List<JoinProject>();

    public virtual ICollection<Report> ReportReportUsers { get; set; } = new List<Report>();

    public virtual ICollection<Report> ReportSendFromNavigations { get; set; } = new List<Report>();

    public virtual ICollection<SendRequest> SendRequests { get; set; } = new List<SendRequest>();

    public virtual ICollection<SubTask> SubTasks { get; set; } = new List<SubTask>();

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
