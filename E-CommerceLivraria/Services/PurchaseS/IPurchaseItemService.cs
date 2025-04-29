using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Services.PurchaseS
{
    public interface IPurchaseItemService
    {
        public PurchaseItem Add(PurchaseItem purchaseItem);
        public PurchaseItem Update(PurchaseItem purchaseItem);
        public bool Delete(PurchaseItem purchaseItem);
        public PurchaseItem? Get(decimal stockId, decimal purchaseId);
        public List<PurchaseItem> GetAll();
    }
}
