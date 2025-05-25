using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace E_CommerceLivraria.Models;

public partial class Country
{
    /// <summary>
    /// Represents a countrys unique identifying value.
    /// </summary>
    public decimal CtrId { get; set; }

    /// <summary>
    /// Represents a countrys name.
    /// </summary>
    public string CtrName { get; set; } = null!;

    public virtual ICollection<State> States { get; set; } = new List<State>();
}
