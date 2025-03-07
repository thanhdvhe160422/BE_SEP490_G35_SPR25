using System;
using System.Collections.Generic;

namespace Planify_BackEnd.Models;

public partial class Event
{
    public int Id { get; set; }

    public string EventTitle { get; set; } = null!;

    public string EventDescription { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public decimal AmountBudget { get; set; }

    public int IsPublic { get; set; }

    public DateTime? TimePublic { get; set; }

    public int Status { get; set; }

    public Guid? ManagerId { get; set; }

    public int CampusId { get; set; }

    public int CategoryEventId { get; set; }

    public string Placed { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public Guid CreateBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? UpdateBy { get; set; }

    public virtual Campus Campus { get; set; } = null!;

    public virtual CategoryEvent CategoryEvent { get; set; } = null!;

    public virtual User CreateByNavigation { get; set; } = null!;

    public virtual ICollection<EventMedium> EventMedia { get; set; } = new List<EventMedium>();

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    public virtual ICollection<JoinProject> JoinProjects { get; set; } = new List<JoinProject>();

    public virtual User? Manager { get; set; }

    public virtual ICollection<SendRequest> SendRequests { get; set; } = new List<SendRequest>();

    public virtual User? UpdateByNavigation { get; set; }
}
