using System;
using System.Collections.Generic;

namespace E_CommerceLivraria.Models;

public partial class PromotionalCoupon
{
    /// <summary>
    /// Represents an unique indentifying value of a coupon
    /// </summary>
    public decimal PcpId { get; set; }

    /// <summary>
    /// Represents the code of a promotional coupon.
    /// </summary>
    public string PcpCode { get; set; } = null!;

    public virtual Coupon Pcp { get; set; } = null!;

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();

    public virtual ICollection<Customer> PcmCtms { get; set; } = new List<Customer>();
}
