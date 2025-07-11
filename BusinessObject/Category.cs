using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Category
{
    public Guid CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Orchid> Orchids { get; set; } = new List<Orchid>();
}
