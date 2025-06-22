using E_CommerceLivraria.DTO.ExchangesDTO;
using E_CommerceLivraria.DTO.PaymentDTO;
using E_CommerceLivraria.DTO.AnalysisDTO;
using E_CommerceLivraria.Enums;
using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Services.PurchaseS
{
    public interface IPurchaseService
    {
        public Purchase Add(PurchaseDataDTO purchaseData);
        public Purchase Update(Purchase purchase);
        public Purchase UpdatePurchaseStatus(Purchase purchase, EStatus newStatus);
        public Purchase UpdateExchangeStatus(Purchase purchase, EStatus newStatus, bool returnStock);
        public bool Delete(decimal id);
        public Purchase? Get(decimal id);
        public List<Purchase> GetAll();
        public Purchase AddExchange(ExchangeRequestDTO exchangeData);
        public List<DataSalesDTO> GetSalesByCategories(DateTime start, DateTime end);
        public List<DataSalesDTO> GetSalesByProduct(DateTime start, DateTime end);
    }
}
