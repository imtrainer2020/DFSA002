using System;
using System.Collections.Generic;

namespace DBO.Models;

public partial class UserRole
{
    public string RoleName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
