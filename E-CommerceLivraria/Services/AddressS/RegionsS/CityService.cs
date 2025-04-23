using E_CommerceLivraria.Models;
using E_CommerceLivraria.Repository.AddressR.RegionsR;

namespace E_CommerceLivraria.Services.AddressS.RegionsS {
    public class CityService : ICityService {
        private readonly ICityRepository _cityRepository;

        public CityService(ICityRepository cityRepository) {
            _cityRepository = cityRepository;
        }

        public City CreateIfNew(City city, State state) {
            var query = _cityRepository.GetAll();
            var result = query.Where(x => x.CtyName == city.CtyName)
                .FirstOrDefault(x => x.CtyStt == state);

            if (result == null) {
                city.CtySttId = city.CtyStt.SttId;
                return _cityRepository.Add(city);
            }
            else {
                return result;
            }
        }
    }
}
