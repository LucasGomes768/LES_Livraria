using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.DTO.ExchangesDTO
{
    public class ExchangeRequestDTO
    {
        public decimal CtmId { get; set; }
        public decimal PrcId { get; set; }
        public List<PurchaseItem> ItemsToExchange {  get; set; } = new List<PurchaseItem>();
    }
}
