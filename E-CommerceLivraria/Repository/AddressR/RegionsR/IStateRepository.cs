using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Repository.AddressR.RegionsR {
    public interface IStateRepository {
        public State Add(State state);
        public State? Get(decimal id);
        public List<State> GetAll();
    }
}
