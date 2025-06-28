using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_CommerceLivraria.Models;

public partial class Customer
{
    /// <summary>
    /// Represents a customers unique indentifying value.
    /// </summary>
    public decimal CtmId { get; set; }

    /// <summary>
    /// Represents a customers name.
    /// </summary>
    public string CtmName { get; set; } = null!;

    /// <summary>
    /// Represents a customers CPF.
    /// </summary>
    public decimal CtmCpf { get; set; }

    [NotMapped]
    public string? CtmCpfStyled {
        set
        {
            string? text = value;
            if (text == null) return;

            decimal cpfTemp;
            text = text.Trim().Replace(".","").Replace("-","");

            if (text.Length != 11) return;
            if (!decimal.TryParse(text, out cpfTemp)) return;

            CtmCpf = cpfTemp;
        }
        get {
            if (CtmCpf == 0) return null;

            string cpf = CtmCpf.ToString();
            return cpf.Substring(0, 3) + "."
                + cpf.Substring(3, 3) + "."
                + cpf.Substring(6, 3) + "-"
                + cpf.Substring(9);
        }
    }

    /// <summary>
    /// Represents a customers e-mail.
    /// </summary>
    public string CtmEmail { get; set; } = null!;

    /// <summary>
    /// Represents a customer accounts password.
    /// </summary>
    public string CtmPass { get; set; } = null!;

    /// <summary>
    /// Boolean value which represents if a customers account is active or not.
    /// </summary>
    public bool CtmActive { get; set; }

    /// <summary>
    /// Represents a customers birthdate.
    /// </summary>
    public DateTime CtmBirthdate { get; set; }

    /// <summary>
    /// Represents a customer&apos;s a numeric ranking based of their purchase profile.
    /// </summary>
    public decimal CtmRanking { get; set; }

    /// <summary>
    /// Represents a customers preffered card ID.
    /// </summary>
    public decimal? CtmPrefferedCrdId { get; set; }

    /// <summary>
    /// Represents a customer&apos;s gender.
    /// </summary>
    public decimal CtmGndId { get; set; }

    /// <summary>
    /// Represents a customer&apos;s home address.
    /// </summary>
    public decimal CtmAddId { get; set; }

    public decimal CtmTlpId { get; set; }

    public decimal? CtmCrtId { get; set; }

    public virtual Cart? Cart { get; set; }

    public virtual Address CtmAdd { get; set; } = null!;

    public virtual Cart? CtmCrt { get; set; } = null!;

    public virtual Gender CtmGnd { get; set; } = null!;

    public virtual CreditCard? CtmPrefferedCrd { get; set; }

    public virtual Telephone CtmTlp { get; set; } = null!;

    public virtual ICollection<ExchangeCoupon> ExchangeCoupons { get; set; } = new List<ExchangeCoupon>();

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();

    public virtual ICollection<Address> BadAdds { get; set; } = new List<Address>();

    public virtual ICollection<CreditCard> CtcCrds { get; set; } = new List<CreditCard>();

    public virtual ICollection<Address> DadAdds { get; set; } = new List<Address>();

    public virtual ICollection<PromotionalCoupon> PcmCpns { get; set; } = new List<PromotionalCoupon>();
}
