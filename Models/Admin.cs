using System;
using System.Collections.Generic;

namespace Planify_BackEnd.Models;

public partial class Admin
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? Avatar { get; set; }

    public int Status { get; set; }

    public virtual MediaItem? AvatarNavigation { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
