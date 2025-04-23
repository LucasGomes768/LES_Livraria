using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_CommerceLivraria.Models;

public partial class Telephone
{
    /// <summary>
    /// Represents a telephones unique identifying value.
    /// </summary>
    public decimal TlpId { get; set; }

    /// <summary>
    /// Represents a telephones number.
    /// </summary>
    public string TlpNumber { get; set; } = null!;

    [NotMapped]
    public string FullNumber {
        get { return $"{TlpDdd}{TlpNumber}"; }
        set {
            if (!string.IsNullOrEmpty(value) && value.Length >= 10) {
                TlpDdd = decimal.Parse(value.Substring(0, 2));
                TlpNumber = value.Substring(2);
            }
        }
    }

    /// <summary>
    /// Represents a telephones DDD.
    /// </summary>
    public decimal TlpDdd { get; set; }

    public decimal TlpTptId { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual TelephoneType TlpTpt { get; set; } = null!;
}
