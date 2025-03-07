using System;
using System.Collections.Generic;

namespace Planify_BackEnd.Models;

public partial class JoinGroup
{
    public int Id { get; set; }

    public Guid ImplementerId { get; set; }

    public int GroupId { get; set; }

    public DateTime TimeJoin { get; set; }

    public DateTime? TimeOut { get; set; }

    public int Status { get; set; }

    public virtual Group Group { get; set; } = null!;

    public virtual User Implementer { get; set; } = null!;
}
