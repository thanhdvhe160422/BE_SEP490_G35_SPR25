using System;
using System.Collections.Generic;

namespace Planify_BackEnd.Models;

public partial class JoinProject
{
    public int Id { get; set; }

    public int? EventId { get; set; }

    public Guid? UserId { get; set; }

    public DateTime? TimeJoinProject { get; set; }

    public DateTime? TimeOutProject { get; set; }

    public int? Role { get; set; }

    public virtual Event? Event { get; set; }

    public virtual User? User { get; set; }
}
