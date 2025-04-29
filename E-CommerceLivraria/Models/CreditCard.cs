using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_CommerceLivraria.Models;

public partial class CreditCard
{
    /// <summary>
    /// Represents a credit cards unique identifying value.
    /// </summary>
    public decimal CrdId { get; set; }

    /// <summary>
    /// Represents a credit cards number.
    /// </summary>
    public decimal CrdNumber { get; set; }

    [NotMapped]
    public string CrdNumberStyled {
        get {
            string cn = CrdNumber.ToString();
            return cn.Substring(0, 4) + "."
                + cn.Substring(4, 4) + "."
                + cn.Substring(8, 4) + "."
                + cn.Substring(12);
        }
    }

    [NotMapped]
    public string CrdNumberHidden
    {
        get
        {
            string cn = CrdNumber.ToString();
            return "****.****.****."
                + cn.Substring(12)
                + " (" + CrdCcf.CcfName + ")";
        }
    }

    /// <summary>
    /// Represents a credit cards printed name.
    /// </summary>
    public string CrdName { get; set; } = null!;

    /// <summary>
    /// Represents a credit cards safety code.
    /// </summary>
    public decimal CrdSafetyCode { get; set; }

    public decimal CrdCcfId { get; set; }

    public virtual CreditCardFlag CrdCcf { get; set; } = null!;

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<CreditCardsPurchase> Purchases { get; set; } = new List<CreditCardsPurchase>();

    public virtual ICollection<Customer> CtcCtms { get; set; } = new List<Customer>();
}
