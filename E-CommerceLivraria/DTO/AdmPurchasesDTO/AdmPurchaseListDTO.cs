using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.DTO.AdmPurchasesDTO
{
    public class AdmPurchaseListDTO
    {
        public List<Purchase> Purchases { get; set; } = new List<Purchase>();
        public decimal? StatusId { get; set; }
    }
}
