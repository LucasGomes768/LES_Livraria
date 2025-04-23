using System;
using System.Collections.Generic;

namespace E_CommerceLivraria.Models;

public partial class Supplier
{
    /// <summary>
    /// Represents a suppliers unique identifying value.
    /// </summary>
    public decimal SppId { get; set; }

    /// <summary>
    /// Represents a suppliers name.
    /// </summary>
    public string SppName { get; set; } = null!;

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
