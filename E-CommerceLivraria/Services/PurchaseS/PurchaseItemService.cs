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

        public PurchaseItem? Get(decimal stockId, decimal purchaseId)
        {
            return _purchaseItemRepository.Get(stockId, purchaseId);
        }

        public List<PurchaseItem> GetAll()
        {
            return _purchaseItemRepository.GetAll();
        }

        public PurchaseItem Update(PurchaseItem purchaseItem)
        {
            return _purchaseItemRepository.Update(purchaseItem);
        }
    }
}
