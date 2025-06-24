using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Repository.CreditCardR
{
    public interface ICreditCardFlagsRepository
    {
        public List<CreditCardFlag> GetAll();
        public CreditCardFlag? Get(decimal id);
    }
}
