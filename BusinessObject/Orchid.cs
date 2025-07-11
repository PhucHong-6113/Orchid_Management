using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Orchid
{
    public Guid OrchidId { get; set; }

    public bool IsNatural { get; set; }

    public string? OrchidDescription { get; set; }

    public string OrchidName { get; set; } = null!;

    public string? OrchidUrl { get; set; }

    public decimal Price { get; set; }

    public Guid CategoryId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
