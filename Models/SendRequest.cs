﻿using System;
using System.Collections.Generic;

namespace Planify_BackEnd.Models;

public partial class SendRequest
{
    public int Id { get; set; }

    public int? EventId { get; set; }

    public Guid? ManagerId { get; set; }

    public string? Reason { get; set; }

    public int Status { get; set; }

    public virtual Event? Event { get; set; }

    public virtual User? Manager { get; set; }
}
