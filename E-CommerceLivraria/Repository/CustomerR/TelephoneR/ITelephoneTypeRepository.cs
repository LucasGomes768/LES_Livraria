using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Repository.CustomerR.TelephoneR {
    public interface ITelephoneTypeRepository {
        public TelephoneType Add(TelephoneType telephoneType);
        public TelephoneType? Get(decimal id);
        public List<TelephoneType> GetAll();
    }
}
