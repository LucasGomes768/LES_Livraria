using E_CommerceLivraria.Models;
using E_CommerceLivraria.Repository.CreditCardR;

namespace E_CommerceLivraria.Services.CreditCardS
{
    public class CreditCardService : ICreditCardService
    {
        private readonly ICreditCardRepository _creditCardRepository;

        public CreditCardService(ICreditCardRepository creditCardRepository)
        {
            _creditCardRepository = creditCardRepository;
        }

        public CreditCard Create(CreditCard creditCard)
        {
            return _creditCardRepository.Create(creditCard);
        }

        public CreditCard? Get(decimal id)
        {
            return _creditCardRepository.Get(id);
        }

        public List<CreditCard> GetAll()
        {
            return _creditCardRepository.GetAll();
        }

        public bool Remove(CreditCard creditCard)
        {
            return _creditCardRepository.Delete(creditCard.CrdId);
        }

        public CreditCard Update(CreditCard creditCard)
        {
            return _creditCardRepository.Update(creditCard);
        }
    }
}
