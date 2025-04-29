using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Services.PurchaseS
{
    public interface IPurchaseService
    {
        public Purchase Add(Purchase purchase);
        public Purchase Update(Purchase purchase);
        public bool Delete(decimal id);
        public Purchase? Get(decimal id);
        public List<Purchase> GetAll();
    }
}
