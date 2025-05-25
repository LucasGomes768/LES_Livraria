using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace E_CommerceLivraria.Models;

public partial class Neighborhood
{
    /// <summary>
    /// Represents a neighborhoods unique identifying value.
    /// </summary>
    public decimal NbhId { get; set; }

    /// <summary>
    /// Represents a neighborhoods name.
    /// </summary>
    public string NbhName { get; set; } = null!;

    public decimal NbhCtyId { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual City NbhCty { get; set; } = null!;
}
