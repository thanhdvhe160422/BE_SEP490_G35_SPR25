using System;
using System.Collections.Generic;

namespace Planify_BackEnd.Models;

public partial class District
{
    public int Id { get; set; }

    public string DistrictName { get; set; } = null!;

    public int? ProvinceId { get; set; }

    public int Status { get; set; }

    public virtual Province? Province { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public virtual ICollection<Ward> Wards { get; set; } = new List<Ward>();
}
