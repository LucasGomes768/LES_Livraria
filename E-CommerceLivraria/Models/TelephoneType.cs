using System;
using System.Collections.Generic;

namespace E_CommerceLivraria.Models;

public partial class TelephoneType
{
    /// <summary>
    /// Represents a telephone types unique identifying value.
    /// </summary>
    public decimal TptId { get; set; }

    /// <summary>
    /// Represents a telephone types name.
    /// </summary>
    public string TptName { get; set; } = null!;

    public virtual ICollection<Telephone> Telephones { get; set; } = new List<Telephone>();
}
