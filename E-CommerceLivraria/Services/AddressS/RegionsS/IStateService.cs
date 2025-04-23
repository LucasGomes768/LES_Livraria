using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Services.AddressS.RegionsS {
    public interface IStateService {
        public State CreateIfNew(State state, Country country);
    }
}
