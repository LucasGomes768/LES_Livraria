using E_CommerceLivraria.DTO.PaymentDTO.Method;
using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.DTO.PaymentDTO
{
    public class FinishPurchaseRequestDTO
    {
        public string PromotionalCode { get; set; } = "";
        public decimal FinalPrice { get; set; }
        public List<decimal> ExchangeIds { get; set; }
        public List<CreditCardPaymentDTO> CreditCards { get; set; }
        public decimal AddressId { get; set; }
        public decimal CtmId { get; set; }
    }
}
