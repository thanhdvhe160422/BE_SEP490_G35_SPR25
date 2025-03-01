using System;
using System.Collections.Generic;

namespace Planify_BackEnd.Models;

public partial class JoinTask
{
    public int Id { get; set; }

    public int? TaskId { get; set; }

    public Guid? ImplementerId { get; set; }

    public DateTime? TimeJoin { get; set; }

    public DateTime? TimeOut { get; set; }

    public int? Status { get; set; }

    public virtual User? Implementer { get; set; }

    public virtual Task? Task { get; set; }
}
