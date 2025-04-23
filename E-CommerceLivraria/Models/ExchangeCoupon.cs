using System;
using System.Collections.Generic;

namespace E_CommerceLivraria.Models;

public partial class ExchangeCoupon
{
    /// <summary>
    /// Represents an unique indentifying value of a coupon
    /// </summary>
    public decimal XcpId { get; set; }

    /// <summary>
    /// Represents the ID of a customer which a exchange coupon belongs to.
    /// </summary>
    public decimal XcpCtmId { get; set; }

    public virtual Coupon Xcp { get; set; } = null!;

    public virtual Customer XcpCtm { get; set; } = null!;

    public virtual ICollection<Purchase> PxcPrcs { get; set; } = new List<Purchase>();
}
