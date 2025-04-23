using System;
using System.Collections.Generic;

namespace E_CommerceLivraria.Models;

public partial class PublicplaceType
{
    /// <summary>
    /// Represents an unique identifying value of a type of public place
    /// </summary>
    public decimal PptId { get; set; }

    /// <summary>
    /// Represents a name of a type of public place.
    /// </summary>
    public string PptName { get; set; } = null!;

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
}
