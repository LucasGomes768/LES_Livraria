using System;
using System.Collections.Generic;

namespace E_CommerceLivraria.Models;

public partial class Publisher
{
    /// <summary>
    /// Represents the unique identifying value of a publisher.
    /// </summary>
    public decimal PblId { get; set; }

    /// <summary>
    /// Represents the name of a publisher.
    /// </summary>
    public string PblName { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
