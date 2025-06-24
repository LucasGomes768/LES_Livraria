using E_CommerceLivraria.Models;
using E_CommerceLivraria.Repository.CreditCardR;

namespace E_CommerceLivraria.Services.CreditCardS
{
    public class CreditCardFlagService : ICreditCardFlagService
    {
        private readonly ICreditCardFlagsRepository _creditCardFlagsRepository;

        public CreditCardFlagService(ICreditCardFlagsRepository creditCardFlagsRepository)
        {
            _creditCardFlagsRepository = creditCardFlagsRepository;
        }

        public CreditCardFlag? Get(decimal id)
        {
            return _creditCardFlagsRepository.Get(id);
        }

        public List<CreditCardFlag> GetAll()
        {
            return _creditCardFlagsRepository.GetAll();
        }
    }
}
