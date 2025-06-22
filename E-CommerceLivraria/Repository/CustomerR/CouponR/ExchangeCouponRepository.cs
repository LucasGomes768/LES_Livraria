using E_CommerceLivraria.Data;
using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Repository.CustomerR.CouponR
{
    public class ExchangeCouponRepository : IExchangeCouponRepository
    {
        private readonly ECommerceDbContext _dbContext;

        public ExchangeCouponRepository(ECommerceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ExchangeCoupon Add(ExchangeCoupon exchangeCoupon)
        {
            _dbContext.ExchangeCoupons.Add(exchangeCoupon);
            _dbContext.SaveChanges();

            return exchangeCoupon;
        }

        public ExchangeCoupon? Get(decimal id)
        {
            return _dbContext.ExchangeCoupons.FirstOrDefault(x => x.XcpId == id);
        }

        public List<ExchangeCoupon> GetAll()
        {
            return _dbContext.ExchangeCoupons.ToList();
        }

        public bool Remove(ExchangeCoupon exchangeCoupon)
        {
            var coupon = Get(exchangeCoupon.XcpId);
            if (coupon != null) throw new Exception("Cupom de troca não foi encontrado");

            _dbContext.ExchangeCoupons.Remove(coupon);
            _dbContext.SaveChanges();

            return true;
        }
    }
}
