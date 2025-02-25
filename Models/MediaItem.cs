﻿using System;
using System.Collections.Generic;

namespace Planify_BackEnd.Models;

public partial class MediaItem
{
    public int Id { get; set; }

    public string MediaUrl { get; set; } = null!;

    public virtual ICollection<Admin> Admins { get; set; } = new List<Admin>();

    public virtual ICollection<EventMedium> EventMedia { get; set; } = new List<EventMedium>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
