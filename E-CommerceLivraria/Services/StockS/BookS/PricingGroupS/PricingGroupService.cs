using E_CommerceLivraria.Models;
using E_CommerceLivraria.Repository.StockR.BookR.PricingGroupR;

namespace E_CommerceLivraria.Services.StockS.BookS.PricingGroupS {
    public class PricingGroupService : IPricingGroupService {
        private IPricingGroupRepository _pricingGroupRepository;

        public PricingGroupService(IPricingGroupRepository pricingGroupRepository) {
            _pricingGroupRepository = pricingGroupRepository;
        }

        public PricingGroup? Get(decimal id) {
            return _pricingGroupRepository.Get(id);
        }

        public List<PricingGroup> GetAll() {
            return _pricingGroupRepository.GetAll();
        }
    }
}
