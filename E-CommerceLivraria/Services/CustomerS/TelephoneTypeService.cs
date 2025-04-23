using E_CommerceLivraria.Models;
using E_CommerceLivraria.Repository.CustomerR;
using E_CommerceLivraria.Repository.CustomerR.TelephoneR;

namespace E_CommerceLivraria.Services.CustomerS {
    public class TelephoneTypeService : ITelephoneTypeService{
        private readonly ITelephoneTypeRepository _telephoneTypeRepository;

        public TelephoneTypeService(ITelephoneTypeRepository telephoneTypeRepository) {
            _telephoneTypeRepository = telephoneTypeRepository;
        }

        public TelephoneType CreateIfNew(TelephoneType telephoneType) {
            var query = GetAll();
            var result = query.FirstOrDefault(x => x.TptName.ToLower() == telephoneType.TptName.ToLower());

            if (result == null) {
                return _telephoneTypeRepository.Add(telephoneType);
            }
            else {
                telephoneType = result;
                return telephoneType;
            }
        }

        public TelephoneType? Get(decimal id) {
            return _telephoneTypeRepository.Get(id);
        }

        public List<TelephoneType> GetAll() {
            return _telephoneTypeRepository.GetAll();
        }
    }
}
