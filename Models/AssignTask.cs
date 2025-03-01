using System;
using System.Collections.Generic;

namespace Planify_BackEnd.Models;

public partial class AssignTask
{
    public int Id { get; set; }

    public Guid? AssignId { get; set; }

    public int? SubTaskId { get; set; }

    public DateTime? TimeJoin { get; set; }

    public DateTime? TimeOut { get; set; }

    public int? Status { get; set; }

    public virtual User? Assign { get; set; }

    public virtual SubTask? SubTask { get; set; }
}
