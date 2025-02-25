using System;
using System.Collections.Generic;

namespace Planify_BackEnd.Models;

public partial class Ward
{
    public int Id { get; set; }

    public string WardName { get; set; } = null!;

    public int? DistrictId { get; set; }

    public int Status { get; set; }

    public virtual District? District { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
