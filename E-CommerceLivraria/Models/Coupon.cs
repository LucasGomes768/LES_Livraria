using System;
using System.Collections.Generic;

namespace E_CommerceLivraria.Models;

public partial class Coupon
{
    /// <summary>
    /// Represents an unique indentifying value of a coupon
    /// </summary>
    public decimal CpnId { get; set; }

    /// <summary>
    /// Represents how much a coupon is worth.
    /// </summary>
    public decimal CpnValue { get; set; }

    /// <summary>
    /// Represents when a coupon was generated.
    /// </summary>
    public DateTime CpnDateGen { get; set; }

    public virtual ExchangeCoupon? ExchangeCoupon { get; set; }

    public virtual PromotionalCoupon? PromotionalCoupon { get; set; }
}
