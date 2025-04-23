using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Services.AddressS.RegionsS {
    public interface INeighborhoodService {
        public Neighborhood CreateIfNew(Neighborhood neighborhood, City city);
    }
}
