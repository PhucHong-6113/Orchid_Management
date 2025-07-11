using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Order
{
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }

    public DateTime OrderDate { get; set; }

    public string OrderStatus { get; set; } = null!;

    public decimal TotalAmount { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
