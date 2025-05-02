using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.DTO.PaymentDTO
{
    public class PurchaseDataDTO
    {
        public FinishPurchaseRequestDTO Request { get; set; }
        public Address DeliveryAddress { get; set; }
        public Customer Customer { get; set; }
    }
}
