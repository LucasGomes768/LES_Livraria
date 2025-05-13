using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Repository.PurchaseR
{
    public interface IPurchaseItemRepository
    {
        public PurchaseItem Add(PurchaseItem purchaseItem);
        public PurchaseItem Update(PurchaseItem purchaseItem);
        public bool Delete(PurchaseItem purchaseItem);
        public PurchaseItem? Get(decimal stockId, decimal purchaseId, decimal status);
        public List<PurchaseItem> GetAll();
    }
}
