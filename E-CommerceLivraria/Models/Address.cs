using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_CommerceLivraria.Models;

public partial class Address
{
    /// <summary>
    /// Represents an unique identifying value of an address.
    /// </summary>
    public decimal AddId { get; set; }

    /// <summary>
    /// Represents a public place nearby the address.
    /// </summary>
    public string AddPublicPlace { get; set; } = null!;

    /// <summary>
    /// Represents an address number.
    /// </summary>
    public decimal AddNumber { get; set; }

    /// <summary>
    /// Represents an address Postal Address Code.
    /// </summary>
    public decimal AddCep { get; set; }

    [NotMapped]
    public string AddCepStyled {
        get {
            string cep = AddCep.ToString();

            if (cep.Length != 8)
                return "";
            else
                return cep.Substring(0, 5) + "-"
                    + cep.Substring(5);
        }
        set
        {
            string cep = value;
            cep = cep.Trim().Replace("-","");

            AddCep = decimal.Parse(cep);
        }
    }

    /// <summary>
    /// Represents a short phrase to better indentify an address.
    /// </summary>
    public string AddShortPhrase { get; set; } = null!;

    /// <summary>
    /// Represents an optional text containing observations regarding an address.
    /// </summary>
    public string? AddObservations { get; set; }

    /// <summary>
    /// Represents the shipping price of an address
    /// </summary>
    public decimal AddShipping { get; set; }

    public decimal AddPptId { get; set; }

    public decimal AddRstId { get; set; }

    public decimal AddNbhId { get; set; }

    public virtual Neighborhood AddNbh { get; set; } = null!;

    public virtual PublicplaceType AddPpt { get; set; } = null!;

    public virtual ResidenceType AddRst { get; set; } = null!;

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();

    public virtual ICollection<Customer> BadCtms { get; set; } = new List<Customer>();

    public virtual ICollection<Customer> DadCtms { get; set; } = new List<Customer>();
}
