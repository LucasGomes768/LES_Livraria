using E_CommerceLivraria.Models;
using E_CommerceLivraria.Repository.CustomerR;
using E_CommerceLivraria.Repository.CustomerR.TelephoneR;

namespace E_CommerceLivraria.Services.CustomerS.TelephoneS {
    public class TelephoneService : ITelephoneService{
        private readonly ITelephoneRepository _telephoneRepository;
        private readonly ITelephoneTypeService _telephoneTypeService;

        public TelephoneService(ITelephoneRepository telephoneRepository,
            ITelephoneTypeService telephoneTypeService) {

            _telephoneRepository = telephoneRepository;
            _telephoneTypeService = telephoneTypeService;
        }

        public Telephone Create(Telephone telephone) {
            telephone.TlpTpt = _telephoneTypeService.CreateIfNew(telephone.TlpTpt);
            return _telephoneRepository.Add(telephone);
        }
    }
}
