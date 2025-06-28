using E_CommerceLivraria.Models;
using E_CommerceLivraria.Repository.CustomerR;
using E_CommerceLivraria.Repository.CustomerR.TelephoneR;

namespace E_CommerceLivraria.Services.CustomerS.TelephoneS {
    public class TelephoneTypeService : ITelephoneTypeService{
        private readonly ITelephoneTypeRepository _telephoneTypeRepository;

        public TelephoneTypeService(ITelephoneTypeRepository telephoneTypeRepository) {
            _telephoneTypeRepository = telephoneTypeRepository;
        }

        public TelephoneType Get(decimal id) {
            return _telephoneTypeRepository.Get(id);
        }

        public List<TelephoneType> GetAll() {
            return _telephoneTypeRepository.GetAll();
        }
    }
}
