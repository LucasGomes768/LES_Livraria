using E_CommerceLivraria.Data;
using E_CommerceLivraria.Models;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceLivraria.Repository.CustomerR.CouponR
{
    public class PromotionalCouponRepository : IPromotionalCouponRepository
    {
        private readonly ECommerceDbContext _dbContext;

        public PromotionalCouponRepository(ECommerceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public PromotionalCoupon Create(PromotionalCoupon promotionalCoupon)
        {
            _dbContext.PromotionalCoupons.Add(promotionalCoupon);
            _dbContext.SaveChanges();

            return promotionalCoupon;
        }

        public PromotionalCoupon? Get(decimal id)
        {
            return _dbContext.PromotionalCoupons
                .Include(x => x.Pcp)
                .Include(x => x.PcmCtms)
                .FirstOrDefault(x => x.PcpId == id);
        }

        public List<PromotionalCoupon> GetAll()
        {
            return _dbContext.PromotionalCoupons
                .Include(x => x.Pcp)
                .Include(x => x.PcmCtms)
                .ToList();
        }

        public PromotionalCoupon Update(PromotionalCoupon promotionalCoupon)
        {
            var Pcp = Get(promotionalCoupon.PcpId);
            if (Pcp == null) throw new Exception("O cupom promocional não foi encontrado");

            _dbContext.PromotionalCoupons.Update(promotionalCoupon);
            _dbContext.SaveChanges();

            return promotionalCoupon;
        }

        public bool Delete(PromotionalCoupon promotionalCoupon)
        {
            _dbContext.PromotionalCoupons.Remove(promotionalCoupon);
            _dbContext.SaveChanges();

            return true;
        }
    }
}
