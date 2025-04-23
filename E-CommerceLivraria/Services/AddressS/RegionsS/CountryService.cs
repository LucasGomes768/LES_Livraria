using E_CommerceLivraria.Models;
using E_CommerceLivraria.Repository.AddressR.RegionsR;

namespace E_CommerceLivraria.Services.AddressS.RegionsS {
    public class CountryService : ICountryService {
        private readonly ICountryRepository _countryRepository;

        public CountryService(ICountryRepository countryRepository) {
            _countryRepository = countryRepository;
        }

        public Country CreateIfNew(Country country) {
            var query = _countryRepository.GetAll();
            var result = query.FirstOrDefault(x => x.CtrName == country.CtrName);

            if (result is null) {
                return _countryRepository.Add(country);
            }
            else {
                country = result;
            }

            return country;
        }
    }
}
