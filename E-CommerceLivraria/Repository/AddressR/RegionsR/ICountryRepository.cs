using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Repository.AddressR.RegionsR {
    public interface ICountryRepository {
        public Country Add(Country country);
        public Country? Get(decimal id);
        public List<Country> GetAll();
    }
}
