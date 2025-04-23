using System;
using System.Collections.Generic;

namespace E_CommerceLivraria.Models;

public partial class Gender
{
    /// <summary>
    /// Represents a genders unique identifying value.
    /// </summary>
    public decimal GndId { get; set; }

    /// <summary>
    /// Represents a genders name.
    /// </summary>
    public string GndName { get; set; } = null!;

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
