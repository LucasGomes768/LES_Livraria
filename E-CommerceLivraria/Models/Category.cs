using System;
using System.Collections.Generic;

namespace E_CommerceLivraria.Models;

public partial class Category
{
    /// <summary>
    /// Represents an unique indentifying value of a books category.
    /// </summary>
    public decimal BctId { get; set; }

    /// <summary>
    /// Represents the name of a books category.
    /// </summary>
    public string BctName { get; set; } = null!;

    public virtual ICollection<Book> BcrBoks { get; set; } = new List<Book>();
}
