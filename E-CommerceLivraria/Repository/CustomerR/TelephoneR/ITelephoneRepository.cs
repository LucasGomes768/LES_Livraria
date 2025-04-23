using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Repository.CustomerR.TelephoneR {
    public interface ITelephoneRepository {
        public Telephone Add(Telephone telephone);
        public Telephone? Get(decimal id);
        public void Remove(Telephone tlp);

    }
}
