using E_CommerceLivraria.Data;
using E_CommerceLivraria.Models;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceLivraria.Repository.PurchaseR
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private ECommerceDbContext _dbContext;

        public PurchaseRepository(ECommerceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Purchase Add(Purchase purchase)
        {
            _dbContext.Purchases.Add(purchase);
            _dbContext.SaveChanges();

            return purchase;
        }

        public bool Delete(decimal id)
        {
            var prc = Get(id);
            if (prc != null) return false;

            _dbContext.Purchases.Remove(prc);
            _dbContext.SaveChanges();

            return true;
        }

        public Purchase? Get(decimal id)
        {
            return _dbContext.Purchases
                .Include(x => x.PxcCpns)
                .Include(x => x.PrcCtm)
                .Include(x => x.PrcAdd)
                    .ThenInclude(x => x.AddRst)
                .Include(x => x.PrcAdd)
                    .ThenInclude(x => x.AddPpt)
                .Include(x => x.PrcAdd)
                    .ThenInclude(x => x.AddNbh)
                        .ThenInclude(x => x.NbhCty)
                            .ThenInclude(x => x.CtyStt)
                                .ThenInclude(x => x.SttCtr)
                .Include(x => x.PrcCpp)
                .Include(x => x.PurchaseItems)
                    .ThenInclude(x => x.PciStc)
                        .ThenInclude(x => x.StcBok)
                .Include(x => x.CreditCards)
                    .ThenInclude(x => x.CcpCrd)
                .FirstOrDefault(x => x.PrcId == id);
        }

        public List<Purchase> GetAll()
        {
            return _dbContext.Purchases
                .Include(x => x.PxcCpns)
                .Include(x => x.PrcCtm)
                .Include(x => x.PrcAdd)
                .Include(x => x.PrcCpp)
                .Include(x => x.PurchaseItems)
                    .ThenInclude(x => x.PciStc)
                        .ThenInclude(x => x.StcBok)
                .Include(x => x.CreditCards)
                    .ThenInclude(x => x.CcpCrd)
                .ToList();
        }

        public Purchase Update(Purchase purchase)
        {
            var prc = _dbContext.Purchases.FirstOrDefault(x => x.PrcId == purchase.PrcId);
            if (prc != null) return purchase;

            _dbContext.Purchases.Update(purchase);
            _dbContext.SaveChanges();

            return purchase;
        }
    }
}
