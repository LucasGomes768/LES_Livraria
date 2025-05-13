using E_CommerceLivraria.Enums;
using E_CommerceLivraria.Models;
using E_CommerceLivraria.Repository.PurchaseR;

namespace E_CommerceLivraria.Services.PurchaseS
{
    public class PurchaseItemService : IPurchaseItemService
    {
        private IPurchaseItemRepository _purchaseItemRepository;

        public PurchaseItemService (IPurchaseItemRepository purchaseItemRepository)
        {
            _purchaseItemRepository = purchaseItemRepository;
        }

        public PurchaseItem Add(PurchaseItem purchaseItem)
        {
            return _purchaseItemRepository.Add(purchaseItem);
        }

        public bool Delete(PurchaseItem purchaseItem)
        {
            return _purchaseItemRepository.Delete(purchaseItem);
        }

        public PurchaseItem? Get(decimal stockId, decimal purchaseId, EStatus status)
        {
            return _purchaseItemRepository.Get(stockId, purchaseId, (decimal)status);
        }

        public List<PurchaseItem> GetAll()
        {
            return _purchaseItemRepository.GetAll();
        }

        public PurchaseItem Update(PurchaseItem purchaseItem)
        {
            var pci = _purchaseItemRepository.Get(purchaseItem.PciStcId, purchaseItem.PciPrcId, purchaseItem.PciStatus);
            if (pci == null) return new PurchaseItem();

            return _purchaseItemRepository.Update(purchaseItem);
        }

        public PurchaseItem UpdateStatus(PurchaseItem purchaseItem, EStatus newStatus)
        {
            if ((EStatus)(int)purchaseItem.PciStatus >= newStatus) return purchaseItem;

            purchaseItem.PciStatus = (decimal)newStatus;
            return _purchaseItemRepository.Update(purchaseItem);
        }
    }
}
