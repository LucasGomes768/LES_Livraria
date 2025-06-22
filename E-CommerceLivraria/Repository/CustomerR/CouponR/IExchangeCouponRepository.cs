using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Repository.CustomerR.CouponR
{
    public interface IExchangeCouponRepository
    {
        public ExchangeCoupon? Get(decimal id);
        public List<ExchangeCoupon> GetAll();
        public bool Remove(ExchangeCoupon exchangeCoupon);
        public ExchangeCoupon Add(ExchangeCoupon exchangeCoupon);

    }
}
