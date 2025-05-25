using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace E_CommerceLivraria.Models;

public partial class State
{
    /// <summary>
    /// Represents a country states unique identifying value.
    /// </summary>
    public decimal SttId { get; set; }

    /// <summary>
    /// Represents the name of a countrys state.
    /// </summary>
    public string SttName { get; set; } = null!;

    public decimal SttCtrId { get; set; }

    public virtual ICollection<City> Cities { get; set; } = new List<City>();

    public virtual Country SttCtr { get; set; } = null!;
}
