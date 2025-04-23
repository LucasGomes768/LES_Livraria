using E_CommerceLivraria.Data;
using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Repository.AddressR {
    public class PublicPlaceTypeRepository : IPublicPlaceTypeRepository{
        private readonly ECommerceDbContext _dbContext;

        public PublicPlaceTypeRepository(ECommerceDbContext dbContext) {
            _dbContext = dbContext;
        }

        public PublicplaceType? Get(decimal id) {
            return _dbContext.PublicplaceTypes.Where(x => x.PptId == id).SingleOrDefault();
        }

        public List<PublicplaceType> GetAll() {
            return _dbContext.PublicplaceTypes.ToList();
        }
    }
}
