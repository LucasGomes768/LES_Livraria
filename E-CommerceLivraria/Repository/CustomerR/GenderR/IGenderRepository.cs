using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Repository.CustomerR.GenderR {
    public interface IGenderRepository {
        public List<Gender> GetAll();
        public Gender? Get(decimal id);
    }
}
