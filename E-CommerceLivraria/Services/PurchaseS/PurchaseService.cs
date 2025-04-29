using E_CommerceLivraria.Models;
using E_CommerceLivraria.Repository.PurchaseR;

namespace E_CommerceLivraria.Services.PurchaseS
{
    public class PurchaseService : IPurchaseService
    {
        private IPurchaseRepository _purchaseRepository;
        private IPurchaseItemService _purchaseItemService;

        public PurchaseService(IPurchaseRepository purchaseRepository, IPurchaseItemService purchaseItemService)
        {
            _purchaseRepository = purchaseRepository;
            _purchaseItemService = purchaseItemService;
        }

        public Purchase Add(Purchase purchase)
        {
            if (!purchase.PurchaseItems.Any() || purchase.PurchaseItems.Count < 1)
                throw new Exception("Compra sem itens");

            return _purchaseRepository.Add(purchase);
        }

        public bool Delete(decimal id)
        {
            return _purchaseRepository.Delete(id);
        }

        public Purchase? Get(decimal id)
        {
            return _purchaseRepository.Get(id);
        }

        public List<Purchase> GetAll()
        {
            return _purchaseRepository.GetAll();
        }

        public Purchase Update(Purchase purchase)
        {
            return _purchaseRepository.Update(purchase);
        }
    }
}
