using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Services.CreditCardS
{
    public interface ICreditCardService
    {
        public CreditCard Create(CreditCard creditCard);
        public CreditCard Create(CreditCard creditCard, Customer customer);
        public CreditCard? Get(decimal id);
        public List<CreditCard> GetAll();
        public CreditCard Update(CreditCard creditCard);
        public bool Remove(CreditCard creditCard);
    }
}
