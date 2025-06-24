using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Services.CreditCardS
{
    public interface ICreditCardFlagService
    {
        public CreditCardFlag? Get(decimal id);
        public List<CreditCardFlag> GetAll();
    }
}
