using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using E_CommerceLivraria.Enums;

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
    public decimal PrcTotalPrice
    {
        get
        {
            var result = PurchaseItems.Where(x => (x.PciStatus < (int)EStatus.EM_TROCA));

            return result.Sum(x => x.PciTotalPrice);
        }
    }

    [NotMapped]
    public decimal PrcExchangeTotalValue
    {
        get
        {
            var result = PurchaseItems.Where(x => !(x.PciStatus >= (int)EStatus.COMPRA_REPROVADA && x.PciStatus < (int)EStatus.TROCA_SOLICITADA));

            return result.Sum(x => x.PciTotalPrice);
        }
    }

    [NotMapped]
    public int PrcStatus
    {
        get
        {
            return (int)PurchaseItems.Min(x => x.PciStatus);
        }
    }

    [NotMapped]
    public int? PrcStatusExchange
    {
        get
        {
            var result = PurchaseItems.Where(x => (x.PciStatus >= (int)EStatus.TROCA_SOLICITADA) || (x.PciStatus == (int)EStatus.TROCA_REPROVADA));

            if (!result.Any() || result.Count() < 1)
            {
                return null;
            } else
            {
                return (int)result.Min(x => x.PciStatus);
            }
        }
    }

    public virtual Address PrcAdd { get; set; } = null!;

    public virtual PromotionalCoupon? PrcCpp { get; set; }

    public virtual Customer PrcCtm { get; set; } = null!;

    public virtual ICollection<CreditCardsPurchase> CreditCards { get; set; } = new List<CreditCardsPurchase>();

    public virtual ICollection<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();

    public virtual ICollection<ExchangeCoupon> PxcCpns { get; set; } = new List<ExchangeCoupon>();
}
