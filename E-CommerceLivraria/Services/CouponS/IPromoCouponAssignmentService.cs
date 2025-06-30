using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Services.CouponS
{
    public interface IPromoCouponAssignmentService
    {
        public Customer AddAllPromoCouponToCtm(Customer customer);
        public PromotionalCoupon AddPromoCouponToAllCtms(PromotionalCoupon promotionalCoupon);
        public Customer RemovePromoCouponFromCtm(Customer customer, PromotionalCoupon promotionalCoupon);
        public Customer RemovePromoCouponFromCtm(Customer customer, string code);
    }
}
