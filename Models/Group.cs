using System;
using System.Collections.Generic;

namespace Planify_BackEnd.Models;

public partial class Group
{
    public int Id { get; set; }

    public string GroupName { get; set; } = null!;

    public Guid CreateBy { get; set; }

    public int EventId { get; set; }

    public decimal AmountBudget { get; set; }

    public virtual User CreateByNavigation { get; set; } = null!;

    public virtual Event Event { get; set; } = null!;

    public virtual ICollection<JoinGroup> JoinGroups { get; set; } = new List<JoinGroup>();

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
