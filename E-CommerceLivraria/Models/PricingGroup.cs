using System;
using System.Collections.Generic;

namespace E_CommerceLivraria.Models;

public partial class PricingGroup
{
    /// <summary>
    /// Represents an unique identifying value of a pricing group.
    /// </summary>
    public decimal PrgId { get; set; }

    /// <summary>
    /// Represents the name of a pricing group.
    /// </summary>
    public string PrgName { get; set; } = null!;

    /// <summary>
    /// Represents a short description of a pricing group.
    /// </summary>
    public string PrgDescription { get; set; } = null!;

    /// <summary>
    /// Represents the profit margin of a pricing group by percentage.
    /// </summary>
    public decimal PrgProfitMargin { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
