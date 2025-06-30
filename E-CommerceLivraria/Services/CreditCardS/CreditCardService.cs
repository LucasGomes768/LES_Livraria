using E_CommerceLivraria.Models;
using E_CommerceLivraria.Repository.CreditCardR;

namespace E_CommerceLivraria.Services.CreditCardS
{
    public class CreditCardService : ICreditCardService
    {
        private readonly ICreditCardRepository _creditCardRepository;
        private readonly ICreditCardFlagService _creditCardFlagService;

        public CreditCardService(ICreditCardRepository creditCardRepository, ICreditCardFlagService creditCardFlagService)
        {
            _creditCardRepository = creditCardRepository;
            _creditCardFlagService = creditCardFlagService;
        }

        public CreditCard Create(CreditCard creditCard)
        {
            if (creditCard == null) throw new ArgumentNullException("Nenhum cartão de crédito foi enviado");

            creditCard.CrdCcf = _creditCardFlagService.Get(creditCard.CrdCcfId);

            return _creditCardRepository.Create(creditCard);
        }

        public CreditCard Create(CreditCard creditCard, Customer customer)
        {
            if (creditCard == null) throw new ArgumentNullException("Nenhum cartão de crédito foi enviado");
            if (customer == null) throw new ArgumentNullException("Nenhum cliente foi enviado");

            creditCard.CtcCtms.Add(customer);
            creditCard.CrdCcf = _creditCardFlagService.Get(creditCard.CrdCcfId);

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
