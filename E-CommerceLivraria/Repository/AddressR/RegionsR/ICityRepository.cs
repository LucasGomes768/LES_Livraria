using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Repository.AddressR.RegionsR {
    public interface ICityRepository {
        public City Add(City city);
        public City? Get(decimal id);
        public List<City> GetAll();
    }
}
