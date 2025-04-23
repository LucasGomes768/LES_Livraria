using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Services.AddressS.RegionsS {
    public interface ICityService {
        public City CreateIfNew(City city, State state);
    }
}
