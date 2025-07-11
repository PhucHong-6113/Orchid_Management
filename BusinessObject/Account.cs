using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Account
{
    public Guid AccountId { get; set; }

    public string AcountName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public Guid RoleId { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Role Role { get; set; } = null!;
}
