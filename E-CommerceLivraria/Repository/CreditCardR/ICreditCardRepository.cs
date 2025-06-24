using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Repository.CreditCardR
{
    public interface ICreditCardRepository
    {
        public CreditCard Create(CreditCard creditCard);
        public CreditCard? Get(decimal id);
        public List<CreditCard> GetAll();
        public CreditCard Update(CreditCard creditCard);
        public bool Delete(decimal id);
    }
}
