using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.DTO.CreditCardDTO
{
    public class CreateCreditCardDTO
    {
        public decimal CtmId { get; set; }
        public int RedirectTo { get; set; }
        public CreditCard creditCard { get; set; } = new CreditCard();
        public bool AddToAccount { get; set; }
    }
}
