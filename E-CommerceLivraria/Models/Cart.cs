using System;
using System.Collections.Generic;

namespace E_CommerceLivraria.Models;

public partial class Cart
{
    /// <summary>
    /// Represents a customer carts unique identifying value
    /// </summary>
    public decimal CrtId { get; set; }

    public decimal CrtCtmId { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Customer CrtCtm { get; set; } = null!;

    public virtual Customer? Customer { get; set; }
}
