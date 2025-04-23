using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Repository.StockR.BookR.PricingGroupR {
    public interface IPricingGroupRepository {
        public PricingGroup? Get(decimal id);
        public List<PricingGroup> GetAll();
    }
}
