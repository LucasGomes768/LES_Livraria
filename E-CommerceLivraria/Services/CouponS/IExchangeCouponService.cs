using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Services.CouponS
{
    public interface IExchangeCouponService
    {
        public ExchangeCoupon? Get(decimal id);
        public List<ExchangeCoupon> GetAllByCtm(Customer customer);
        public ExchangeCoupon AddToCtm(Customer customer, decimal value);
        public void RemoveFromCtm(Customer customer, ExchangeCoupon coupon);
        public void RemoveFromCtm(Customer customer, List<ExchangeCoupon> coupons);
    }
}
