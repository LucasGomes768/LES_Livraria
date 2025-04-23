using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Repository.AddressR.RegionsR {
    public interface INeighborhoodRepository {
        public Neighborhood Add(Neighborhood neighborhood);
        public Neighborhood? Get(decimal id);
        public List<Neighborhood> GetAll();
    }
}
