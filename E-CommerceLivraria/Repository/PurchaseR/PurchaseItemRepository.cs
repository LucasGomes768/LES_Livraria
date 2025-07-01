
using E_CommerceLivraria.Data;
using E_CommerceLivraria.Models;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceLivraria.Repository.PurchaseR
{
    public class PurchaseItemRepository : IPurchaseItemRepository
    {
        private readonly ECommerceDbContext _dbContext;

        public PurchaseItemRepository(ECommerceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public PurchaseItem Add(PurchaseItem purchaseItem)
        {
            _dbContext.PurchaseItems.Add(purchaseItem);
            _dbContext.SaveChanges();

            return purchaseItem;
        }

        public bool Delete(PurchaseItem purchaseItem)
        {
            var pci = _dbContext.PurchaseItems.FirstOrDefault(x => (x.PciStcId == purchaseItem.PciStcId) && (x.PciPrcId == purchaseItem.PciPrcId) && (x.PciStatus == purchaseItem.PciStatus));
            if (pci == null) return false;

            _dbContext.PurchaseItems.Remove(pci);
            _dbContext.SaveChanges();

            return true;
        }

        public PurchaseItem? Get(decimal stockId, decimal purchaseId, decimal status)
        {
            return _dbContext.PurchaseItems
                .Include(x => x.PciStc)
                    .ThenInclude(x => x.StcBok)
                        .ThenInclude(x => x.BokPrg)
                .Include(x => x.PciPrc)
                .FirstOrDefault(x => (x.PciStcId == stockId) && (x.PciPrcId == purchaseId) && (x.PciStatus == status));
        }

        public List<PurchaseItem> GetAll()
        {
            return _dbContext.PurchaseItems
                .Include(x => x.PciStc)
                .Include(x => x.PciPrc)
                .ToList();
        }

        public PurchaseItem Update(PurchaseItem purchaseItem)
        {
            _dbContext.PurchaseItems.Update(purchaseItem);
            _dbContext.SaveChanges();

            return purchaseItem;
        }
    }
}
