﻿using System;
using System.Collections.Generic;

namespace Planify_BackEnd.Models;

public partial class Role
{
    public int Id { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<JoinProject> JoinProjects { get; set; } = new List<JoinProject>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
