using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Services.AddressS.RegionsS {
    public interface ICountryService {
        public Country CreateIfNew(Country country);
    }
}
