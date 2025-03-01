using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Planify_BackEnd.Models;

public partial class Event
{
    public int Id { get; set; }

    public string EventTitle { get; set; } = null!;

    public string? EventDescription { get; set; }

    public Guid? CreateBy { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public DateTime? TimeOfEvent { get; set; }

    public DateTime? EndOfEvent { get; set; }

    public DateTime? CreatedAt { get; set; }

    public decimal? AmountBudget { get; set; }

    public int? IsPublic { get; set; }

    public DateTime? TimePublic { get; set; }

    public int? Status { get; set; }

    public Guid? ManagerId { get; set; }

    public int? CampusId { get; set; }

    public int? CategoryEventId { get; set; }

    public string? Placed { get; set; }

    public virtual Campus? Campus { get; set; }

    [JsonIgnore]
    public virtual CategoryEvent? CategoryEvent { get; set; }

    public virtual User? CreateByNavigation { get; set; }

    public virtual ICollection<EventMedium> EventMedia { get; set; } = new List<EventMedium>();

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    public virtual ICollection<JoinProject> JoinProjects { get; set; } = new List<JoinProject>();

    public virtual User? Manager { get; set; }

    public virtual ICollection<SendRequest> SendRequests { get; set; } = new List<SendRequest>();
}
