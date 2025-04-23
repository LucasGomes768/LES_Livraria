using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_CommerceLivraria.Models;

public partial class CartItem
{
    public decimal CriCrtId { get; set; }

    public decimal CriStcId { get; set; }

    /// <summary>
    /// Represents the amount of instances of a product are in a cart
    /// </summary>
    public decimal CriQuantity { get; set; }

    /// <summary>
    /// Represents the total price of a product in a cart.
    /// </summary>
    public decimal CriTotalprice { get; set; }

    /// <summary>
    /// Represents the last time a item in a cart was altered.
    /// </summary>
    public DateTime CriLastTimeAltered { get; set; }

    public virtual Cart CriCrt { get; set; } = null!;

    public virtual Stock CriStc { get; set; } = null!;
}
