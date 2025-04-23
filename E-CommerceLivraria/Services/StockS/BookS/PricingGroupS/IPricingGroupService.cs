using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Services.StockS.BookS.PricingGroupS {
    public interface IPricingGroupService {
        public PricingGroup? Get(decimal id);
        public List<PricingGroup> GetAll();
    }
}
