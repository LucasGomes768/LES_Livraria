using E_CommerceLivraria.Data;
using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Repository.StockR.BookR.PricingGroupR {
    public class PricingGroupRepository : IPricingGroupRepository {
        private readonly ECommerceDbContext _dbContext;

        public PricingGroupRepository(ECommerceDbContext dbContext) {
            _dbContext = dbContext;
        }

        public PricingGroup? Get(decimal id) {
            return _dbContext.PricingGroups.FirstOrDefault(x => x.PrgId == id);
        }

        public List<PricingGroup> GetAll() {
            return _dbContext.PricingGroups.ToList();
        }
    }
}
