using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_CommerceLivraria.Models;

public partial class Stock
{
    /// <summary>
    /// Represents an unique identifying value of a product in stock.
    /// </summary>
    public decimal StcId { get; set; }

    /// <summary>
    /// Represents the amount of items of a product in stock available for purchase.
    /// </summary>
    public decimal StcAvailableAmount { get; set; }

    /// <summary>
    /// Represents the amount of items of a product in stock which are in a customer&apos;s cart.
    /// </summary>
    public decimal StcBlockedAmount { get; set; }

    /// <summary>
    /// Represents a products cost value in dollars.
    /// </summary>
    public decimal StcCost { get; set; }

    /// <summary>
    /// Represents when a product was registered.
    /// </summary>
    public DateTime StcEntryDate { get; set; }

    /// <summary>
    /// Represents, in minutes, how long it takes until an item is removed from a cart.
    /// </summary>
    public decimal StcRemoveInterval { get; set; }

    /// <summary>
    /// Represents a products supplier unique identifying value.
    /// </summary>
    public decimal StcSppId { get; set; }

    /// <summary>
    /// Represents which books stock is been referenced in a row.
    /// </summary>
    public decimal StcBokId { get; set; }

    [NotMapped]
    public decimal StcSalePrice {
        get {
            return StcCost * (1 + StcBok.BokPrg.PrgProfitMargin);
        }
    }

    public virtual Book? Book { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();

    public virtual Book StcBok { get; set; } = null!;

    public virtual Supplier StcSpp { get; set; } = null!;
}
