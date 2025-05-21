using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Services.CustomerS.TelephoneS {
    public interface ITelephoneTypeService {
        public TelephoneType CreateIfNew(TelephoneType telephoneType);
        public TelephoneType? Get(decimal id);
        public List<TelephoneType> GetAll();
    }
}
