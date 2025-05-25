using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace E_CommerceLivraria.Models;

public partial class City
{
    /// <summary>
    /// Represents an unique identifying value of a city.
    /// </summary>
    public decimal CtyId { get; set; }

    /// <summary>
    /// Represents a name of a city.
    /// </summary>
    public string CtyName { get; set; } = null!;

    public decimal CtySttId { get; set; }

    public virtual State CtyStt { get; set; } = null!;

    public virtual ICollection<Neighborhood> Neighborhoods { get; set; } = new List<Neighborhood>();
}
