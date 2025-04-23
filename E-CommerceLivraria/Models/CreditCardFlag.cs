using System;
using System.Collections.Generic;

namespace E_CommerceLivraria.Models;

public partial class CreditCardFlag
{
    /// <summary>
    /// Represents a credit card flags unique identifying value.
    /// </summary>
    public decimal CcfId { get; set; }

    /// <summary>
    /// Represents a credit card flags name.
    /// </summary>
    public string CcfName { get; set; } = null!;

    public virtual ICollection<CreditCard> CreditCards { get; set; } = new List<CreditCard>();
}
