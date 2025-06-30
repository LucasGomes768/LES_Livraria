using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Services.CouponS
{
    public interface IPromotionalCouponService
    {
        public PromotionalCoupon Create(decimal value, string? code = null);
        public PromotionalCoupon? GetByCode(string code);
        public List<PromotionalCoupon> GetAllActive();
        public List<PromotionalCoupon> GetAll();
        public PromotionalCoupon Update(PromotionalCoupon coupon);
        public PromotionalCoupon RemoveFromAccount(Customer customer, PromotionalCoupon coupon);
        public PromotionalCoupon Deactivate(decimal id);

    }
}
