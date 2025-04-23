using System;
using System.Collections.Generic;

namespace E_CommerceLivraria.Models;

public partial class PurchaseItem
{
    public decimal PciPrcId { get; set; }

    public decimal PciStcId { get; set; }

    public decimal PciQuantity { get; set; }

    public decimal PciTotalPrice { get; set; }

    public decimal PciStatus { get; set; }

    public virtual Purchase PciPrc { get; set; } = null!;

    public virtual Stock PciStc { get; set; } = null!;
}
