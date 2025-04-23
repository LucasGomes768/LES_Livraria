using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Repository.AddressR {
    public interface IPublicPlaceTypeRepository {
        public List<PublicplaceType> GetAll();
        public PublicplaceType? Get(decimal id);
    }
}
