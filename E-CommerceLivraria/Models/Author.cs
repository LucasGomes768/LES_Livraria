using System;
using System.Collections.Generic;

namespace E_CommerceLivraria.Models;

public partial class Author
{
    /// <summary>
    /// Represents an authors unique indentifying value.
    /// </summary>
    public decimal BatId { get; set; }

    /// <summary>
    /// Represents an authors full name.
    /// </summary>
    public string BatName { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
