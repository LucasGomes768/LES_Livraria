using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Repository.CustomerR.CouponR
{
    public interface IPromotionalCouponRepository
    {
        public PromotionalCoupon Create(PromotionalCoupon promotionalCoupon);
        public PromotionalCoupon? Get(decimal id);
        public List<PromotionalCoupon> GetAll();
        public PromotionalCoupon Update(PromotionalCoupon promotionalCoupon);
        public bool Delete(PromotionalCoupon promotionalCoupon);
    }
}
