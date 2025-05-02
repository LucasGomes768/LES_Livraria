using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_CommerceLivraria.Models;

public partial class Purchase
{
    /// <summary>
    /// Represents a purchases unique identifying value.
    /// </summary>
    public decimal PrcId { get; set; }

    /// <summary>
    /// Represents when a purchase was made.
    /// </summary>
    public DateTime PrcDate { get; set; }

    /// <summary>
    /// Represents a promotional coupons foreign key.
    /// </summary>
    public decimal? PrcCppId { get; set; }

    public decimal PrcAddId { get; set; }

    public decimal PrcCtmId { get; set; }

    /// <summary>
    /// Represents the total price of all products in a purchase.
    /// </summary>
    [NotMapped]
    public decimal PrcTotalValue
    {
        get
        {
            return PurchaseItems.Sum(x => x.PciTotalPrice);
        }
    }

    public virtual Address PrcAdd { get; set; } = null!;

    public virtual PromotionalCoupon? PrcCpp { get; set; }

    public virtual Customer PrcCtm { get; set; } = null!;

    public virtual ICollection<CreditCardsPurchase> CreditCards { get; set; } = new List<CreditCardsPurchase>();

    public virtual ICollection<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();

    public virtual ICollection<ExchangeCoupon> PxcCpns { get; set; } = new List<ExchangeCoupon>();
}
