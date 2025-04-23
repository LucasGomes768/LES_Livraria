using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Repository.AddressR {
    public interface IResidenceTypeRepository {
        public List<ResidenceType> GetAll();
        public ResidenceType? Get(decimal id);
    }
}
