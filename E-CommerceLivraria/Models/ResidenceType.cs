using System;
using System.Collections.Generic;

namespace E_CommerceLivraria.Models;

public partial class ResidenceType
{
    /// <summary>
    /// Represents an unique identifying value of a residence type.
    /// </summary>
    public decimal RstId { get; set; }

    /// <summary>
    /// Represents the name of a residence type.
    /// </summary>
    public string RstName { get; set; } = null!;

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
}
