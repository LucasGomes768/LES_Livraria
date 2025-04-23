using System;
using System.Collections.Generic;

namespace E_CommerceLivraria.Models;

public partial class Book
{
    /// <summary>
    /// Represents an unique identifying value of a book.
    /// </summary>
    public decimal BokId { get; set; }

    /// <summary>
    /// Represents a books International Standard Book Number.
    /// </summary>
    public decimal BokIsbn { get; set; }

    /// <summary>
    /// Represents a book barcodes numbers.
    /// </summary>
    public decimal BokBarcode { get; set; }

    /// <summary>
    /// Represents the title of a book.
    /// </summary>
    public string BokTitle { get; set; } = null!;

    /// <summary>
    /// Represents a short description of what book is about.
    /// </summary>
    public string BokSinopsis { get; set; } = null!;

    /// <summary>
    /// Represents the year when the book was published.
    /// </summary>
    public decimal BokYear { get; set; }

    /// <summary>
    /// Represents the books edition.
    /// </summary>
    public decimal BokEdition { get; set; }

    /// <summary>
    /// Represents the amount of pages a book has.
    /// </summary>
    public decimal BokPagesAmount { get; set; }

    /// <summary>
    /// Represents a books height in centimeters.
    /// </summary>
    public decimal BokHeight { get; set; }

    /// <summary>
    /// Represents a books length in centimeters.
    /// </summary>
    public decimal BokLength { get; set; }

    /// <summary>
    /// Represents a books weight in grams.
    /// </summary>
    public decimal BokWeight { get; set; }

    /// <summary>
    /// Represents a books depth in centimeters.
    /// </summary>
    public decimal BokDepth { get; set; }

    /// <summary>
    /// Represents the local address of a books cover image.
    /// </summary>
    public string? BokImgAddress { get; set; }

    public decimal BokBatId { get; set; }

    public decimal BokPblId { get; set; }

    public decimal BokPrgId { get; set; }

    public decimal BokStcId { get; set; }

    public virtual Author BokBat { get; set; } = null!;

    public virtual Publisher BokPbl { get; set; } = null!;

    public virtual PricingGroup BokPrg { get; set; } = null!;

    public virtual Stock BokStc { get; set; } = null!;

    public virtual Stock? Stock { get; set; }

    public virtual ICollection<Category> BcrBcts { get; set; } = new List<Category>();
}
