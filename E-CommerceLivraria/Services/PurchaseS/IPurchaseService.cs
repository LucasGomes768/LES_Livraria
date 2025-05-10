using E_CommerceLivraria.DTO.PaymentDTO;
using E_CommerceLivraria.Enums;
using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Services.PurchaseS
{
    public interface IPurchaseService
    {
        public Purchase Add(PurchaseDataDTO purchaseData);
        public Purchase Update(Purchase purchase);
        public Purchase UpdateStatus(Purchase purchase, EStatus newStatus);
        public bool Delete(decimal id);
        public Purchase? Get(decimal id);
        public List<Purchase> GetAll();
    }
}
